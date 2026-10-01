using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;
public interface ICommentService
{
    Task<Result<CommentResponse>> CreateAsync(CreateCommentRequest request, int currentUserId);
    Task<Result> DeleteAsync(int id, int currentUserId);
    Task<Result<List<CommentResponse>>> GetByAuthorIdAsync(int authorId, int currentUserId);
    Task<Result<CommentResponse>> GetByIdAsync(int id, int currentUserId);
    Task<Result<List<CommentResponse>>> GetByIssueIdAsync(int issueId, int currentUserId);
    Task<Result<List<CommentResponse>>> GetByPullRequestIdAsync(int pullRequestId, int currentUserId);
    Task<Result<CommentResponse>> UpdateAsync(int id, UpdateCommentRequest request, int currentUserId);
}
