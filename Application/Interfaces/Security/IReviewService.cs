using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Security;
public interface IReviewService
{
    Task<Result<List<ReviewResponse>>> GetByPullRequestIdAsync(int pullRequestId, int currentUserId);
    Task<Result<List<ReviewResponse>>> GetByReviewerIdAsync(int reviewerId);
    Task<Result<ReviewResponse>> GetByIdAsync(int id, int currentUserId);

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
