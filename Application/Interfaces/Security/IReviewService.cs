using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Security;
public interface IReviewService
{
    Task<Result<List<ReviewResponse>>> GetByPullRequestIdAsync(int pullRequestId);
    Task<Result<List<ReviewResponse>>> GetByReviewerIdAsync(int reviewerId);
    Task<Result<ReviewResponse>> GetByIdAsync(int id);

    Task<Result<ReviewResponse>> CreateAsync(
        int pullRequestId,
        CreateReviewRequest request,
        int currentUserId);

    Task<Result<ReviewResponse>> UpdateAsync(
        int id,
        UpdateReviewRequest request,
        int currentUserId);

    Task<Result> DeleteAsync(int id, int currentUserId);
}
