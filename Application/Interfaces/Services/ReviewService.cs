using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Security;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Services;

public class ReviewService(IUnitOfWork unitOfWork, IMapper mapper) : IReviewService
{
    public async Task<Result<ReviewResponse>> CreateAsync(int pullRequestId, CreateReviewRequest request, int currentUserId)
    {
        var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(pullRequestId);

        if(pullRequest == null)
            return Result<ReviewResponse>.Failure("Pull request not found", ErrorType.NotFound);

        var repository = await unitOfWork.Repositories.GetByIdAsync(pullRequest.RepositoryId);

        if (repository == null)
            return Result<ReviewResponse>.Failure("Repository not found.",ErrorType.NotFound);

        var membership = await unitOfWork.RepositoryMembers.GetMembershipAsync(pullRequest.RepositoryId,currentUserId);

        if (membership == null)
            return Result<ReviewResponse>.Failure("You don't have permission to review this pull request.",ErrorType.Forbidden);

        if (membership.Permission == RepositoryPermission.Viewer)
        {
            return Result<ReviewResponse>.Failure(
                "You don't have permission to review this pull request.",
                ErrorType.Forbidden);
        }

        var user = await unitOfWork.Users.GetByIdAsync(currentUserId);

        if(user == null)
            return Result<ReviewResponse>.Failure("User not found", ErrorType.NotFound);

        var existingReview = await unitOfWork.Reviews.GetByPullRequestAndReviewerAsync(pullRequestId, currentUserId);

        if(existingReview != null)
            return Result<ReviewResponse>.Failure("You have already submitted a review for this pull request", ErrorType.Conflict);

        var review = mapper.Map<Review>(request);
        review.PullRequestId = pullRequestId;
        review.ReviewerId = currentUserId;

        await unitOfWork.Reviews.AddASync(review);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<ReviewResponse>(review);

        return Result<ReviewResponse>.Success(response);
    }

    public async Task<Result> DeleteAsync(int id, int currentUserId)
    {
        var review = await unitOfWork.Reviews.GetByIdAsync(id);

        if(review == null)
            return Result.Failure("Review not found", ErrorType.NotFound);

        if(review.ReviewerId != currentUserId)
            return Result.Failure("You are not allowed to delete this review", ErrorType.Forbidden);

        unitOfWork.Reviews.Delete(review);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<ReviewResponse>> GetByIdAsync(int id, int currentUserId)
    {
        var review = await unitOfWork.Reviews.GetByIdAsync(id);

        if(review == null)
            return Result<ReviewResponse>.Failure("Review not found", ErrorType.NotFound);

        var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(review.PullRequestId);

        if (pullRequest == null)
            return Result<ReviewResponse>.Failure("Pull request not found.",ErrorType.NotFound);

        var repository = await unitOfWork.Repositories.GetByIdAsync(pullRequest.RepositoryId);

        if (repository == null)
            return Result<ReviewResponse>.Failure("Repository not found.",ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers
                .GetMembershipAsync(
                    repository.Id,
                    currentUserId);

            if (membership is null)
            {
                return Result<ReviewResponse>.Failure(
                    "You do not have permission to view this review.",
                    ErrorType.Forbidden);
            }
        }

        var response = mapper.Map<ReviewResponse>(review);

        return Result<ReviewResponse>.Success(response);
    }

    public async Task<Result<List<ReviewResponse>>> GetByPullRequestIdAsync(int pullRequestId, int currentUserId)
    {
        var pullRequest = await unitOfWork.PullRequests.GetByIdAsync(pullRequestId);

        if(pullRequest == null)
            return Result<List<ReviewResponse>>.Failure("Pull request not found", ErrorType.NotFound);

        var repository = await unitOfWork.Repositories.GetByIdAsync(pullRequest.RepositoryId);

        if (repository == null)
            return Result<List<ReviewResponse>>.Failure("Repository not found.",ErrorType.NotFound);

        if (repository.Visibility == RepositoryVisibility.Private)
        {
            var membership = await unitOfWork.RepositoryMembers
                .GetMembershipAsync(
                    repository.Id,
                    currentUserId);

            if (membership is null)
            {
                return Result<List<ReviewResponse>>.Failure(
                    "You do not have permission to view these reviews.",
                    ErrorType.Forbidden);
            }
        }

        var reviews = await unitOfWork.Reviews.GetByPullRequestIdAsync(pullRequestId);

        var response = mapper.Map<List<ReviewResponse>>(reviews);

        return Result<List<ReviewResponse>>.Success(response);
    }

    public async Task<Result<List<ReviewResponse>>> GetByReviewerIdAsync(int reviewerId)
    {
        var user = await unitOfWork.Users.GetByIdAsync(reviewerId);

        if (user == null)
            return Result<List<ReviewResponse>>.Failure("user not found", ErrorType.NotFound);

        var review = await unitOfWork.Reviews.GetByReviewerIdAsync(reviewerId);

        var response = mapper.Map<List<ReviewResponse>>(review);

        return Result<List<ReviewResponse>>.Success(response);
    }

    public async Task<Result<ReviewResponse>> UpdateAsync(int id, UpdateReviewRequest request, int currentUserId)
    {
        var review = await unitOfWork.Reviews.GetByIdAsync(id);

        if (review == null)
            return Result<ReviewResponse>.Failure("Review not found", ErrorType.NotFound);

        if (review.ReviewerId != currentUserId)
            return Result<ReviewResponse>.Failure(
               "You are not allowed to update this review.",ErrorType.Forbidden);

        mapper.Map(request, review);

        unitOfWork.Reviews.Update(review);
        await unitOfWork.SaveChangesAsync();

        var response = mapper.Map<ReviewResponse>(review);

        return Result<ReviewResponse>.Success(response);
    }
}
