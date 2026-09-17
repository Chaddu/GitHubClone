using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;
public interface IPullRequestService
{
    Task<Result<List<PullRequestResponse>>> GetByRepositoryIdAsync(int repositoryId);
    Task<Result<List<PullRequestResponse>>> GetByAuthorIdAsync(int authorId);
    Task<Result<PullRequestResponse>> GetByIdAsync(int id);
    Task<Result<PullRequestResponse>> CreateAsync(int repositoryId,CreatePullRequestRequest request,int currentUserId);

    Task<Result<PullRequestResponse>> UpdateAsync(int id,UpdatePullRequestRequest request,int currentUserId);

    Task<Result> DeleteAsync(int id, int currentUserId);
}
