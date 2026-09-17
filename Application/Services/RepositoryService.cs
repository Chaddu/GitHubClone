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
        var exists = await unitOfWork.Repositories.ExistsByNameAsync(request.Name);
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

        if(repo == null)
            return Result.Failure("Repository not found.", ErrorType.NotFound);

        if (repo.OwnerId != currentUserId)
            return Result.Failure("You don't have permission to delete this repository.", ErrorType.Forbidden);

        unitOfWork.Repositories.Delete(repo);
        await unitOfWork.SaveChangesAsync();

        var respone = mapper.Map<RepositoryResponse>(repo);

        return Result<RepositoryResponse>.Success(respone, "Repository deleted successfully.");
    }

    public async Task<Result<RepositoryResponse>> GetByIdAsync(int id)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(id);

        if(repo == null)
            return Result<RepositoryResponse>.Failure("Repository not found.", ErrorType.NotFound);

        var response = mapper.Map<RepositoryResponse>(repo);

        return Result<RepositoryResponse>.Success(response, "Repository retrieved successfully.");
    }

    public async Task<Result<RepositoryResponse>> GetByNameAsync(string name)
    {
        var repo = await unitOfWork.Repositories.GetByNameAsync(name);

        if(repo == null)
            return Result<RepositoryResponse>.Failure("Repository not found.", ErrorType.NotFound);

        var response = mapper.Map<RepositoryResponse>(repo);

        return Result<RepositoryResponse>.Success(response, "Repository retrieved successfully.");
    }

    public async Task<Result<List<RepositoryResponse>>> GetByOwnerIdAsync(int ownerId)
    {
        var repo = await unitOfWork.Repositories.GetByOwnerIdAsync(ownerId);

        if(repo == null)
            return Result<List<RepositoryResponse>>.Failure("No repositories found for this owner.", ErrorType.NotFound);

        var response = mapper.Map<List<RepositoryResponse>>(repo);

        return Result<List<RepositoryResponse>>.Success(response, "Repositories retrieved successfully.");
    }

    public async Task<Result<RepositoryResponse>> UpdateAsync(int id, UpdateRepositoryRequest request, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(id);

        if(repo == null)
            return Result<RepositoryResponse>.Failure("Repository not found.", ErrorType.NotFound);

        if(repo.OwnerId != currentUserId)
            return Result<RepositoryResponse>.Failure("You don't have permission to update this repository.", ErrorType.Forbidden);

        if(repo.Name != request.Name)
        {
            var exists = await unitOfWork.Repositories.ExistsByNameAsync(request.Name);
            if (exists)
                return Result<RepositoryResponse>.Failure("Repository with the same name already exists.", ErrorType.Conflict);
        }

        mapper.Map<RepositoryResponse>(repo);

        unitOfWork.Repositories.Update(repo);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<RepositoryResponse>(repo);

        return Result<RepositoryResponse>.Success(response, "Repository updated successfully.");
    }
}
