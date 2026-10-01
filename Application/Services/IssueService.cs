using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class IssueService(IUnitOfWork unitOfWork, IMapper mapper) : IIssueService
{
    public async Task<Result<IssueResponse>> CreateAsync(int repositoryId, CreateIssueRequest request, int currentUserId)
    {
        var repo = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if(repo == null)
            return Result<IssueResponse>.Failure("Repository not found.", ErrorType.NotFound);
        if (repo.OwnerId != currentUserId)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repositoryId, currentUserId);

            if (membership == null)
                return Result<IssueResponse>.Failure("You are not a member of this repository");

            if (membership.Permission == RepositoryPermission.Viewer)
                return Result<IssueResponse>.Failure("You don't have permission to create issues.", ErrorType.Forbidden);

        }
        

        var issue = mapper.Map<Issue>(request);

        issue.RepositoryId = repositoryId;
        issue.CreatorId = currentUserId;
        issue.Status = IssueStatus.Open;

        await unitOfWork.Issues.AddASync(issue);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<IssueResponse>(issue);

        return Result<IssueResponse>.Success(response);

    }

    public async Task<Result> DeleteAsync(int id, int currentUserId)
    {
        var issue = await unitOfWork.Issues.GetByIdAsync(id);

        if(issue == null)
            return Result.Failure("Issue not found.", ErrorType.NotFound);

        var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(issue.RepositoryId, currentUserId);

        if (membership == null)
            return Result.Failure("You don't have permission to delete this issue.",ErrorType.Forbidden);

        if (membership.Permission != RepositoryPermission.Admin &&
            membership.Permission != RepositoryPermission.Maintainer &&
            issue.CreatorId != currentUserId)
        {
            return Result.Failure(
                "You don't have permission to delete this issue.",
                ErrorType.Forbidden);
        }

        unitOfWork.Issues.Delete(issue);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<List<IssueResponse>>> GetByAuthorIdAsync(int authorId)
    {
        var issues = await unitOfWork.Issues.GetByAuthorIdAsync(authorId);

        var response = mapper.Map<List<IssueResponse>>(issues);

        return Result<List<IssueResponse>>.Success(response);
    }

    public async Task<Result<IssueResponse>> GetByIdAsync(int id,int currentUserId)
    {
        var issue = await unitOfWork.Issues.GetByIdAsync(id);

        if (issue is null)
            return Result<IssueResponse>.Failure("Issue not found.",ErrorType.NotFound);

        var repository = await unitOfWork.Repositories.GetByIdAsync(issue.RepositoryId);

        if (repository is null)
            return Result<IssueResponse>.Failure("Repository not found.",ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers
                .GetMembershipAsync(
                    issue.RepositoryId,
                    currentUserId);

            if (membership is null)
            {
                return Result<IssueResponse>.Failure(
                    "You do not have permission to view this issue.",
                    ErrorType.Forbidden);
            }
        }

        var response = mapper.Map<IssueResponse>(issue);

        return Result<IssueResponse>.Success(response);
    }


    public async Task<Result<List<IssueResponse>>> GetByRepositoryIdAsync(int repositoryId,int currentUserId)
    {
        var repository = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repository is null)
            return Result<List<IssueResponse>>.Failure("Repository not found.",ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers
                .GetMembershipAsync(
                    repositoryId,
                    currentUserId);

            if (membership is null)
            {
                return Result<List<IssueResponse>>.Failure(
                    "You do not have permission to view this repository.",
                    ErrorType.Forbidden);
            }
        }

        var issues = await unitOfWork.Issues.GetByRepositoryIdAsync(repositoryId);

        var response = mapper.Map<List<IssueResponse>>(issues);

        return Result<List<IssueResponse>>.Success(response);
    }

    public async Task<Result<List<IssueResponse>>> GetOpenIssuesAsync(int repositoryId,int currentUserId)
    {
        var repository = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repository is null)
            return Result<List<IssueResponse>>.Failure("Repository not found.",ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers
                .GetMembershipAsync(
                    repositoryId,
                    currentUserId);

            if (membership is null)
            {
                return Result<List<IssueResponse>>.Failure(
                    "You do not have permission to view this repository.",
                    ErrorType.Forbidden);
            }
        }

        var issues = await unitOfWork.Issues.GetOpenIssuesByRepositoryIdAsync(repositoryId);

        var response = mapper.Map<List<IssueResponse>>(issues);

        return Result<List<IssueResponse>>.Success(response);
    }

    public async Task<Result<IssueResponse>> UpdateAsync(int id, UpdateIssueRequest request, int currentUserId)
    {
        var issue = await unitOfWork.Issues.GetByIdAsync(id);

        if(issue == null)
            return Result<IssueResponse>.Failure("Issue not found.", ErrorType.NotFound);

        var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(issue.RepositoryId, currentUserId);

        if (membership == null)
            return Result<IssueResponse>.Failure("You don't have permission to update this issue.", ErrorType.Forbidden);

        if (membership.Permission != RepositoryPermission.Admin &&
            membership.Permission != RepositoryPermission.Maintainer &&
            issue.CreatorId != currentUserId)
        {
            return Result<IssueResponse>.Failure(
                "You don't have permission to update this issue.",
                ErrorType.Forbidden);
        }

        mapper.Map(request, issue);

        unitOfWork.Issues.Update(issue);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<IssueResponse>(issue);

        return Result<IssueResponse>.Success(response);
    }

}
