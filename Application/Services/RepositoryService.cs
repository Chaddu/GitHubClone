using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Enums;

namespace Application.Services;

public class RepositoryService(IUnitOfWork unitOfWork, IMapper mapper) : IRepositoryService
{
    public async Task<Result<RepositoryResponse>> CreateAsync(CreateRepositoryRequest request, int currentUserId)
    {
        var exists = await unitOfWork.Repositories
    .ExistsByNameAsync(
        request.Name,
        request.OrganizationId == null ? currentUserId : null,
        request.OrganizationId);
        if (exists)
            return Result<RepositoryResponse>.Failure("Repository with the same name already exists.", ErrorType.Conflict);

        var repository = mapper.Map<Domain.Entities.Repository>(request);


        if (request.OrganizationId == null)
        {
            repository.OwnerId = currentUserId;
        }
        else
        {
            var membership = await unitOfWork.OrganizationMembers
                .GetMembershipAsync(
                    request.OrganizationId.Value,
                    currentUserId);
            if (membership is null)
            {
                return Result<RepositoryResponse>.Failure(
                    "You are not a member of this organization.",
                    ErrorType.Forbidden);
            }

            if (membership.Role != OrganizationRole.Owner &&
                membership.Role != OrganizationRole.Admin)
            {
                return Result<RepositoryResponse>.Failure(
                    "You don't have permission to create repositories in this organization.",
                    ErrorType.Forbidden);
            }

            repository.OwnerId = null;
            repository.OrganizationId = request.OrganizationId;

        }

        await unitOfWork.Repositories.AddASync(repository);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<RepositoryResponse>(repository);

        return Result<RepositoryResponse>.Success(response, "Repository created successfully.");

    }

    public async Task<Result> DeleteAsync(int id, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(id);

        if (repo == null)
            return Result.Failure("Repository not found.",ErrorType.NotFound);
        

        var hasPermission = await HasManagePermissionAsync(repo,currentUserId);

        if (!hasPermission)
            return Result.Failure("You don't have permission to delete this repository.",ErrorType.Forbidden);

        unitOfWork.Repositories.Delete(repo);
        await unitOfWork.SaveChangesAsync();

        return Result.Success("Repository deleted successfully.");
    }

    public async Task<Result<RepositoryResponse>> GetByIdAsync(int id,int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(id);

        if (repo == null)
            return Result<RepositoryResponse>.Failure("Repository not found.",ErrorType.NotFound);
        

        var hasPermission = await HasViewPermissionAsync(repo,currentUserId);

        if (!hasPermission)
            return Result<RepositoryResponse>.Failure("You don't have permission to view this repository.",ErrorType.Forbidden);
        

        var response = mapper.Map<RepositoryResponse>(repo);

        return Result<RepositoryResponse>.Success(response,"Repository retrieved successfully.");
    }

    public async Task<Result<List<RepositoryResponse>>> GetByOwnerIdAsync(int ownerId)
    {
        var repositories = await unitOfWork.Repositories.GetByOwnerIdAsync(ownerId);

        if (repositories == null || repositories.Count == 0)
            return Result<List<RepositoryResponse>>.Failure("No repositories found for this owner.",ErrorType.NotFound);

        var response = mapper.Map<List<RepositoryResponse>>(repositories);

        return Result<List<RepositoryResponse>>.Success(response,"Repositories retrieved successfully.");
    }

    public async Task<Result<RepositoryResponse>> GetByNameAsync(string name,int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByNameAsync(name);

        if (repo == null)
            return Result<RepositoryResponse>.Failure("Repository not found.",ErrorType.NotFound);

        var hasPermission = await HasViewPermissionAsync(repo,currentUserId);

        if (!hasPermission)
            return Result<RepositoryResponse>.Failure("You don't have permission to view this repository.",ErrorType.Forbidden);

        var response = mapper.Map<RepositoryResponse>(repo);

        return Result<RepositoryResponse>.Success(response,"Repository retrieved successfully.");
    }

    public async Task<Result<RepositoryResponse>> UpdateAsync(
    int id,
    UpdateRepositoryRequest request,
    int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(id);

        if (repo == null)
            return Result<RepositoryResponse>.Failure("Repository not found.", ErrorType.NotFound);

        var hasPermission = await HasManagePermissionAsync(repo,currentUserId);

        if (!hasPermission)
            return Result<RepositoryResponse>.Failure("You don't have permission to update this repository.",ErrorType.Forbidden);

        if (repo.Name != request.Name)
        {
            var exists = await unitOfWork.Repositories
                .ExistsByNameAsync(request.Name);

            if (exists)
            {
                return Result<RepositoryResponse>.Failure(
                    "Repository with the same name already exists.",
                    ErrorType.Conflict);
            }
        }

        mapper.Map(request, repo);

        unitOfWork.Repositories.Update(repo);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<RepositoryResponse>(repo);

        return Result<RepositoryResponse>.Success(response,"Repository updated successfully.");
    }

    public async Task<Result<List<RepositoryResponse>>> GetByOrganizationIdAsync(int organizationId)
    {
        var organization = await unitOfWork.Organizations.GetByIdAsync(organizationId);

        if (organization == null)
            return Result<List<RepositoryResponse>>.Failure("Organization was not found", ErrorType.NotFound);

        var repo = await unitOfWork.Repositories.GetByOrganizationIdAsync(organizationId);

        var response = mapper.Map<List<RepositoryResponse>>(repo);

        return Result<List<RepositoryResponse>>.Success(response);
    }

    private async Task<bool> HasManagePermissionAsync(
    Domain.Entities.Repository repository,
    int currentUserId)
    {
        if (repository.OwnerId == currentUserId)
            return true;

        if (repository.OrganizationId != null)
        {
            var organizationMembership = 
                await unitOfWork.OrganizationMembers.GetMembershipAsync(
                    repository.OrganizationId.Value,
                    currentUserId);

            if (organizationMembership is not null &&(organizationMembership.Role == OrganizationRole.Owner ||
                organizationMembership.Role == OrganizationRole.Admin))
                 return true;
            
        }

        var repositoryMembership =
            await unitOfWork.RepositoryMembers.GetMembershipAsync(
                repository.Id,
                currentUserId);

        return repositoryMembership?.Permission == RepositoryPermission.Admin;
    }


    private async Task<bool> HasViewPermissionAsync(
    Domain.Entities.Repository repository,
    int currentUserId)
    {
        if (repository.Visibility == RepositoryVisibility.Public)
            return true;

        if (repository.OwnerId == currentUserId)
            return true;

        if (repository.OrganizationId != null)
        {
            var organizationMembership =
                await unitOfWork.OrganizationMembers.GetMembershipAsync(
                    repository.OrganizationId.Value,
                    currentUserId);

            if (organizationMembership != null)
                return true;
        }

        var repositoryMembership =
            await unitOfWork.RepositoryMembers.GetMembershipAsync(
                repository.Id,
                currentUserId);

        return repositoryMembership != null;
    }

   
}
