using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;
public interface IIssueService
{
    Task<Result<List<IssueResponse>>> GetByRepositoryIdAsync(int repositoryId, int currentUserId);
    Task<Result<List<IssueResponse>>> GetOpenIssuesAsync(int repositoryId, int currentUserId);
    Task<Result<List<IssueResponse>>> GetByAuthorIdAsync(int authorId);
    Task<Result<IssueResponse>> GetByIdAsync(int id, int currentUserId);
    Task<Result<IssueResponse>> CreateAsync(int repositoryId, CreateIssueRequest request, int currentUserId);
    Task<Result<IssueResponse>> UpdateAsync(int id, UpdateIssueRequest request, int currentUserId);
    Task<Result> DeleteAsync(int id, int currentUserId);
}
