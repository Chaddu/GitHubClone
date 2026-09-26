using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class PullRequestService(IUnitOfWork unitOfWork, IMapper mapper) : IPullRequestService
{
    public async Task<Result<PullRequestResponse>> CreateAsync(int repositoryId, CreatePullRequestRequest request, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repo == null)
            return Result<PullRequestResponse>.Failure("Repository not found", ErrorType.NotFound);

        var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repositoryId, currentUserId);

        if (membership == null)
            return Result<PullRequestResponse>.Failure("You are not a member of this repository.",ErrorType.Forbidden);

        if (membership.Permission == RepositoryPermission.Viewer)
            return Result<PullRequestResponse>.Failure("You don't have permission to create pull requests.", ErrorType.Forbidden);

        var sourceBranch = await unitOfWork.Branches.GetByIdAsync(request.SourceBranchId);

        if (sourceBranch == null || sourceBranch.RepositoryId != repositoryId)
            return Result<PullRequestResponse>.Failure("Source branch was not found in this repository.", ErrorType.NotFound);

        var targetBranch = await unitOfWork.Branches.GetByIdAsync(request.TargetBranchId);
        if (targetBranch == null || targetBranch.RepositoryId != repositoryId)
            return Result<PullRequestResponse>.Failure("Target branch was not found in this repository.", ErrorType.NotFound);

        if (request.SourceBranchId == request.TargetBranchId)
            return Result<PullRequestResponse>.Failure("Source and target branches cannot be the same.", ErrorType.BadRequest);

        var pullRequest = mapper.Map<PullRequest>(request);

        pullRequest.RepositoryId = repositoryId;
        pullRequest.AuthorId = currentUserId;
        pullRequest.Status = PullRequestStatus.Open;

        await unitOfWork.PullRequests.AddASync(pullRequest);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<PullRequestResponse>(pullRequest);

        return Result<PullRequestResponse>.Success(response, "Pull request created successfully.");


    }

    public async Task<Result> DeleteAsync(int id, int currentUserId)
    {
        var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(id);

        if (pullRequest is null)
            return Result.Failure("Pull request not found.",ErrorType.NotFound);

        var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(pullRequest.RepositoryId,currentUserId);

        if (membership == null)
            return Result.Failure("You don't have permission to delete this pull request.",ErrorType.Forbidden);

        if (membership.Permission != RepositoryPermission.Admin &&
            membership.Permission != RepositoryPermission.Maintainer &&
            pullRequest.AuthorId != currentUserId)
        {
            return Result.Failure("You don't have permission to delete this pull request.",ErrorType.Forbidden);
        }

        unitOfWork.PullRequests.Delete(pullRequest);

        await unitOfWork.SaveChangesAsync();

        return Result.Success("Pull request deleted successfully.");
    }

    public async Task<Result<List<PullRequestResponse>>> GetByAuthorIdAsync(int authorId,int currentUserId)
    {
        var user = await unitOfWork.Users.GetByIdAsync(authorId);

        if (user == null)
            return Result<List<PullRequestResponse>>.Failure("User not found.",ErrorType.NotFound);

        var pullRequests = await unitOfWork.PullRequests.GetByAuthorIdAsync(authorId);

        if (pullRequests == null)
            return Result<List<PullRequestResponse>>.Success(new List<PullRequestResponse>());

        var visiblePullRequests = new List<PullRequest>();

        foreach (var pullRequest in pullRequests)
        {
            var repository = await unitOfWork.Repositories.GetByIdAsync(pullRequest.RepositoryId);

            if (repository is null)
                continue;

            if (repository.Visibility == RepositoryVisibility.Public)
            {
                visiblePullRequests.Add(pullRequest);
                continue;
            }

            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(pullRequest.RepositoryId,currentUserId);

            if (membership != null)
            {
                visiblePullRequests.Add(pullRequest);
            }
        }

        var response = mapper.Map<List<PullRequestResponse>>(visiblePullRequests);

        return Result<List<PullRequestResponse>>.Success(response);
    }

    public async Task<Result<PullRequestResponse>> GetByIdAsync(int id, int currentUserId)
    {
        var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(id);

        if (pullRequest == null)
            return Result<PullRequestResponse>.Failure("Pull request not found", ErrorType.NotFound);
        var repository = await unitOfWork.Repositories.GetByIdAsync(pullRequest.RepositoryId);

        if (repository == null)
            return Result<PullRequestResponse>.Failure( "Repository not found.", ErrorType.NotFound);


        if (repository.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(pullRequest.RepositoryId,currentUserId);

            if (membership == null)
            {
                return Result<PullRequestResponse>.Failure("You do not have permission to view this pull request.", ErrorType.Forbidden);
            }
        }

        var response = mapper.Map<PullRequestResponse>(pullRequest);
        return Result<PullRequestResponse>.Success(response, "Pull request retrieved successfully.");
    }

    public async Task<Result<List<PullRequestResponse>>> GetByRepositoryIdAsync(int repositoryId, int currentUserId)
    {
        var repository = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repository == null)
            return Result<List<PullRequestResponse>>.Failure("Repository not found", ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repositoryId,currentUserId);

            if (membership == null)
            {
                return Result<List<PullRequestResponse>>.Failure("You do not have permission to view this repository.",ErrorType.Forbidden);
            }
        }

        var pullRequest = await unitOfWork.PullRequests.GetByRepositoryIdAsync(repositoryId);

        if (pullRequest == null)
            return Result<List<PullRequestResponse>>.Failure("No pull requests found for this repository.", ErrorType.NotFound);

        var response = mapper.Map<List<PullRequestResponse>>(pullRequest);

        return Result<List<PullRequestResponse>>.Success(response);

    }

    public async Task<Result<PullRequestResponse>> UpdateAsync(int id, UpdatePullRequestRequest request, int currentUserId)
    {
        var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(id);

        if (pullRequest == null)
            return Result<PullRequestResponse>.Failure("Pull request not found", ErrorType.NotFound);
        if (pullRequest.AuthorId != currentUserId)
            return Result<PullRequestResponse>.Failure("You are not allowed to update this pull request.", ErrorType.Forbidden);

        if (pullRequest.Status != PullRequestStatus.Open)
            return Result<PullRequestResponse>.Failure("Only open pull requests can be updated.", ErrorType.BadRequest);

        mapper.Map(request, pullRequest);

        unitOfWork.PullRequests.Update(pullRequest);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<PullRequestResponse>(pullRequest);
        return Result<PullRequestResponse>.Success(response);
    }
}
