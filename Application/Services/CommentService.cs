using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System.ComponentModel;

namespace Application.Services;

public class CommentService(IUnitOfWork unitOfWork, IMapper mapper) : ICommentService
{
    public async Task<Result<CommentResponse>> CreateAsync(CreateCommentRequest request, int currentUserId)
    {
        if (request.IssueId.HasValue == request.PullRequestId.HasValue)
            return Result<CommentResponse>.Failure("A comment must belong to either an issue or a pull request.", ErrorType.BadRequest);

        int repositoryId;

        if (request.IssueId.HasValue)
        {
            var issue = await unitOfWork.Issues.GetByIdAsync(request.IssueId.Value);

            if (issue == null)
                return Result<CommentResponse>.Failure("Issue not found.", ErrorType.NotFound);

            repositoryId = issue.RepositoryId;
        }
        else
        {
            var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(request.PullRequestId!.Value);

            if (pullRequest == null)
                return Result<CommentResponse>.Failure("Pull request was not found.", ErrorType.NotFound);

            repositoryId = pullRequest.RepositoryId;
        }

        var repository = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repository == null)
            return Result<CommentResponse>.Failure("Repository not found.", ErrorType.NotFound);

        if (repository.OwnerId != currentUserId)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repositoryId, currentUserId);

            if (membership == null)
                return Result<CommentResponse>.Failure("You are not a member of this repository.", ErrorType.Forbidden);

            if (membership.Permission == RepositoryPermission.Viewer)
                return Result<CommentResponse>.Failure("You don't have permission to create comments.", ErrorType.Forbidden);
        }

        var comment = mapper.Map<Comment>(request);

        comment.AuthorId = currentUserId;

        await unitOfWork.Comments.AddASync(comment);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<CommentResponse>(comment);

        return Result<CommentResponse>.Success(response, "Comment created successfully.");
    }

    public async Task<Result> DeleteAsync(int id, int currentUserId)
    {
        var comment = await unitOfWork.Comments.GetByIdAsync(id);

        if (comment == null)
            return Result.Failure("Comment was not found.", ErrorType.NotFound);

        int repositoryId;

        if (comment.IssueId.HasValue)
        {
            var issue = await unitOfWork.Issues.GetByIdAsync(comment.IssueId.Value);

            if (issue == null)
                return Result.Failure("Issue not found.", ErrorType.NotFound);

            repositoryId = issue.RepositoryId;
        }
        else if (comment.PullRequestId.HasValue)
        {
            var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(comment.PullRequestId.Value);

            if (pullRequest == null)
                return Result.Failure("Pull request was not found.", ErrorType.NotFound);

            repositoryId = pullRequest.RepositoryId;
        }
        else
        {
            return Result.Failure("Comment is not associated with an issue or pull request.", ErrorType.BadRequest);
        }

        var repository = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repository == null)
            return Result.Failure("Repository not found.", ErrorType.NotFound);

        if (repository.OwnerId != currentUserId)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repositoryId, currentUserId);

            if (membership == null)
                return Result.Failure("You are not a member of this repository.", ErrorType.Forbidden);

            if (membership.Permission != RepositoryPermission.Admin &&
                membership.Permission != RepositoryPermission.Maintainer &&
                comment.AuthorId != currentUserId)
            {
                return Result.Failure("You don't have permission to delete this comment.", ErrorType.Forbidden);
            }
        }

        unitOfWork.Comments.Delete(comment);
        await unitOfWork.SaveChangesAsync();

        return Result.Success("Comment deleted successfully.");
    }

    public async Task<Result<List<CommentResponse>>> GetByAuthorIdAsync(int authorId, int currentUserId)
    {
        var user = await unitOfWork.Users.GetByIdAsync(authorId);

        if (user == null)
            return Result<List<CommentResponse>>.Failure("User was not found", ErrorType.NotFound);

        var comments = await unitOfWork.Comments.GetByAuthorIdAsync(authorId);

        var visibleComments = new List<Comment>();

        foreach (var comment in comments)
        {
            int repositoryId;

            if (comment.IssueId.HasValue)
            {
                var issue = await unitOfWork.Issues.GetByIdAsync(comment.IssueId.Value);

                if (issue == null)
                    continue;

                repositoryId = issue.RepositoryId;
            }
            else if (comment.PullRequestId.HasValue)
            {
                var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(comment.PullRequestId.Value);

                if (pullRequest == null)
                    continue;

                repositoryId = pullRequest.RepositoryId;
            }
            else
            {
                continue;
            }

            var repository = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

            if (repository == null)
                continue;

            if (repository.Visibility == RepositoryVisibility.Public ||
                repository.OwnerId == currentUserId)
            {
                visibleComments.Add(comment);
                continue;
            }

            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repositoryId, currentUserId);

            if (membership != null)
                visibleComments.Add(comment);
        }

        var response = mapper.Map<List<CommentResponse>>(visibleComments);

        return Result<List<CommentResponse>>.Success(response);

    }

    public async Task<Result<CommentResponse>> GetByIdAsync(int id, int currentUserId)
    {
        var comment = await unitOfWork.Comments.GetByIdAsync(id);

        if (comment == null)
            return Result<CommentResponse>.Failure("Comment was not found", ErrorType.NotFound);

        int repositoryId;

        if (comment.IssueId.HasValue)
        {
            var issue = await unitOfWork.Issues.GetByIdAsync(comment.IssueId.Value);

            if (issue == null)
                return Result<CommentResponse>.Failure("Issue not found.", ErrorType.NotFound);

            repositoryId = issue.RepositoryId;
        }
        else if (comment.PullRequestId.HasValue)
        {
            var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(comment.PullRequestId.Value);

            if (pullRequest == null)
                return Result<CommentResponse>.Failure("Pull request was not found.", ErrorType.NotFound);

            repositoryId = pullRequest.RepositoryId;
        }
        else
        {
            return Result<CommentResponse>.Failure("Comment is not associated with an issue or pull request.", ErrorType.BadRequest);
        }

        var repository = await unitOfWork.Repositories.GetByIdAsync(repositoryId);

        if (repository == null)
            return Result<CommentResponse>.Failure("Repository not found.", ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private &&
            repository.OwnerId != currentUserId)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repositoryId, currentUserId);

            if (membership == null)
                return Result<CommentResponse>.Failure("You do not have permission to view this comment.", ErrorType.Forbidden);
        }

        var response = mapper.Map<CommentResponse>(comment);

        return Result<CommentResponse>.Success(response);
    }

    public async Task<Result<List<CommentResponse>>> GetByIssueIdAsync(int issueId, int currentUserId)
    {
        var issue = await unitOfWork.Issues.GetByIdAsync(issueId);

        if (issue == null)
            return Result<List<CommentResponse>>.Failure("Issue not found", ErrorType.NotFound);

        var repository = await unitOfWork.Repositories.GetByIdAsync(issue.RepositoryId);

        if (repository == null)
            return Result<List<CommentResponse>>.Failure("Repository not found.", ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private &&
            repository.OwnerId != currentUserId)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repository.Id, currentUserId);

            if (membership == null)
                return Result<List<CommentResponse>>.Failure("You do not have permission to view these comments.", ErrorType.Forbidden);
        }

        var comments = await unitOfWork.Comments.GetByIssueIdAsync(issueId);

        var response = mapper.Map<List<CommentResponse>>(comments);

        return Result<List<CommentResponse>>.Success(response);

    }

    public async Task<Result<List<CommentResponse>>> GetByPullRequestIdAsync(int pullRequestId, int currentUserId)
    {
        var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(pullRequestId);

        if (pullRequest == null)
            return Result<List<CommentResponse>>.Failure("Pull request was not found", ErrorType.NotFound);

        var repository = await unitOfWork.Repositories.GetByIdAsync(pullRequest.RepositoryId);

        if (repository == null)
            return Result<List<CommentResponse>>.Failure("Repository not found.", ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private &&
            repository.OwnerId != currentUserId)
        {
            var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(repository.Id, currentUserId);

            if (membership == null)
                return Result<List<CommentResponse>>.Failure("You do not have permission to view these comments.", ErrorType.Forbidden);
        }

        var comments = await unitOfWork.Comments.GetByPullRequestIdAsync(pullRequestId);

        var response = mapper.Map<List<CommentResponse>>(comments);

        return Result<List<CommentResponse>>.Success(response);
    }

    public async Task<Result<CommentResponse>> UpdateAsync(int id, UpdateCommentRequest request, int currentUserId)
    {
        var comment = await unitOfWork.Comments.GetByIdAsync(id);

        if (comment == null)
            return Result<CommentResponse>.Failure("Comment was not found",ErrorType.NotFound);

        if (comment.AuthorId != currentUserId)
            return Result<CommentResponse>.Failure("You are not allowed to update this comment",ErrorType.Forbidden);

        mapper.Map(request, comment);

        unitOfWork.Comments.Update(comment);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<CommentResponse>(comment);

        return Result<CommentResponse>.Success(response);
    }
}
