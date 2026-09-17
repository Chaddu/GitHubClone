using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Enums;

namespace Application.Services;

public class CommentService(IUnitOfWork unitOfWork, IMapper mapper) : ICommentService
{
    public Task<Result<CommentResponse>> CreateAsync(CreateCommentRequest request, int currentUserId)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(int id, int currentUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<CommentResponse>>> GetByAuthorIdAsync(int authorId)
    {
        var user = await unitOfWork.Users.GetByIdAsync(authorId);

        if (user == null)
            return Result<List<CommentResponse>>.Failure("User was not found", ErrorType.NotFound);

        var comments = await unitOfWork.Comments.GetByAuthorIdAsync(authorId);


        var response = mapper.Map<List<CommentResponse>>(comments);
        return Result<List<CommentResponse>>.Success(response);

    }

    public async Task<Result<CommentResponse>> GetByIdAsync(int id)
    {
        var comment = await unitOfWork.Comments.GetByIdAsync(id);

        if (comment == null)
            return Result<CommentResponse>.Failure("Comment was not found", ErrorType.NotFound);

        var response = mapper.Map<CommentResponse>(comment);

        return Result<CommentResponse>.Success(response);
    }

    public async Task<Result<List<CommentResponse>>> GetByIssueIdAsync(int issueId)
    {
        var issue = await unitOfWork.Issues.GetByIdAsync(issueId);

        if (issue == null)
            return Result<List<CommentResponse>>.Failure("Issue not found", ErrorType.NotFound);

        var comment = await unitOfWork.Comments.GetByIssueIdAsync(issueId);

        var response = mapper.Map<List<CommentResponse>>(comment);

        return Result<List<CommentResponse>>.Success(response);

    }

    public async Task<Result<List<CommentResponse>>> GetByPullRequestIdAsync(int pullRequestId)
    {
        var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(pullRequestId);
        if (pullRequest == null)
            return Result<List<CommentResponse>>.Failure("Pull request was not found", ErrorType.NotFound);

        var comment = await unitOfWork.Comments.GetByPullRequestIdAsync(pullRequestId);

        var response = mapper.Map<List<CommentResponse>>(comment);

        return Result<List<CommentResponse>>.Success(response);
    }

    public async Task<Result<CommentResponse>> UpdateAsync(int id, UpdateCommentRequest request, int currentUserId)
    {
        var comment = await unitOfWork.Comments.GetByIdAsync(id);

        if (comment == null)
            return Result<CommentResponse>.Failure("Comment was not found");

        if(comment.AuthorId != currentUserId)
            return Result<CommentResponse>.Failure("You are not allowed to update this comment", ErrorType.Forbidden);

        mapper.Map(request, comment);

        unitOfWork.Comments.Update(comment);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<CommentResponse>(comment);

        return Result<CommentResponse>.Success(response);
    }
}
