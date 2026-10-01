using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class BranchService(IUnitOfWork unitOfWork, IMapper mapper) : IBranchService
{
    public async Task<Result<BranchResponse>> CreateAsync(int repositoryId, CreateBranchRequest request, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if(repo == null)
            return Result<BranchResponse>.Failure("Repository not found", ErrorType.NotFound);

        if (repo.OwnerId != currentUserId)
        {
            var repositoryMembership =
                await unitOfWork.RepositoryMembers.GetMembershipAsync(
                    repositoryId,
                    currentUserId);

            if (repositoryMembership is null)
                return Result<BranchResponse>.Failure(
                    "You are not a member of this repository.",
                    ErrorType.Forbidden);

            if (repositoryMembership.Permission == RepositoryPermission.Viewer)
                return Result<BranchResponse>.Failure(
                    "You don't have permission to create branches.",
                    ErrorType.Forbidden);
        }

        var exists = await unitOfWork.Branches.ExistsByNameAsync(repositoryId, request.Name);

        if(exists)
            return Result<BranchResponse>.Failure("Branch with the same name already exists", ErrorType.Conflict);

        var branch = mapper.Map<Branch>(request);

        branch.RepositoryId = repositoryId;
        branch.CreatorId = currentUserId;

        await unitOfWork.Branches.AddASync(branch);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<BranchResponse>(branch);

        return Result<BranchResponse>.Success(response);
    }

    public async Task<Result> DeleteAsync(int id, int currentUserId)
    {
        var branch = await unitOfWork.Branches.GetByIdAsync(id);

        if (branch == null)
            return Result.Failure("Branch not found", ErrorType.NotFound);

        var canManage = await CanManageBranchAsync(branch, currentUserId);

        if (!canManage)
            return Result.Failure("You do not have permission to delete this branch.",ErrorType.Forbidden);

        unitOfWork.Branches.Delete(branch);
        await unitOfWork.SaveChangesAsync();
        
        return Result.Success("Branch deleted successfully");
    }

    public async Task<Result<BranchResponse>> GetByNameAsync(int repositoryId, string name, int currentUserId)
    {
        var repository = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repository == null)
            return Result<BranchResponse>.Failure("Repository was not found.",ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers
                .GetMembershipAsync(repositoryId, currentUserId);

            if (membership == null)
            {
                return Result<BranchResponse>.Failure(
                    "You do not have permission to view this repository.",
                    ErrorType.Forbidden);
            }
        }

        var branch = await unitOfWork.Branches.GetByNameAsync(repositoryId, name);

        if(branch == null)
            return Result<BranchResponse>.Failure("Branch not found", ErrorType.NotFound);
        var response = mapper.Map<BranchResponse>(branch);

        return Result<BranchResponse>.Success(response);
    }

    public async Task<Result<List<BranchResponse>>> GetByRepositoryIdAsync(int repositoryId, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(repositoryId);
        if(repo == null)
            return Result<List<BranchResponse>>.Failure("Repository not found", ErrorType.NotFound);

        if (repo.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers
                .GetMembershipAsync(repositoryId, currentUserId);

            if (membership == null)
            {
                return Result<List<BranchResponse>>.Failure(
                    "You do not have permission to view this repository.",
                    ErrorType.Forbidden);
            }
        }
        var branches = await unitOfWork.Branches.GetByRepositoryIdAsync(repositoryId);

        if(branches == null)
            return Result<List<BranchResponse>>.Failure("Branches not found", ErrorType.NotFound);

        var response = mapper.Map<List<BranchResponse>>(branches);

        return Result<List<BranchResponse>>.Success(response);
    }

    public async Task<Result<BranchResponse>> UpdateAsync(int id, UpdateBranchRequest request, int currentUserId)
    {
        var branch = await unitOfWork.Branches.GetByIdAsync(id);
        if(branch == null)
            return Result<BranchResponse>.Failure("Branch not found", ErrorType.NotFound);

        var canManage = await CanManageBranchAsync(branch,currentUserId);

        if (!canManage)
            return Result<BranchResponse>.Failure("You do not have permission to update this branch.",ErrorType.Forbidden);

        if (branch.Name != request.Name)
        {
            var exists = await unitOfWork.Branches.ExistsByNameAsync(branch.RepositoryId, request.Name);
            if (exists)
                return Result<BranchResponse>.Failure("Branch with the same name already exists", ErrorType.Conflict);
        }

        mapper.Map(request, branch);
        unitOfWork.Branches.Update(branch);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<BranchResponse>(branch);

        return Result<BranchResponse>.Success(response);
    }

    private async Task<bool> CanManageBranchAsync(
    Branch branch,
    int currentUserId)
    {
        var repositoryMembership =
            await unitOfWork.RepositoryMembers.GetMembershipAsync(
                branch.RepositoryId,
                currentUserId);

        if (repositoryMembership is null)
            return false;

        return repositoryMembership.Permission == RepositoryPermission.Admin ||
               repositoryMembership.Permission == RepositoryPermission.Maintainer;
    }
}
