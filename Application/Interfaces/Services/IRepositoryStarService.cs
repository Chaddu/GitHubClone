using Application.Common;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;
public interface IRepositoryStarService
{
    Task<Result<List<RepositoryStarResponse>>> GetByRepositoryIdAsync(int repositoryId, int currentUserId);
    Task<Result<List<RepositoryStarResponse>>> GetByUserIdAsync(int userId);
    Task<Result<RepositoryStarResponse>> StarAsync(int repositoryId, int currentUserId);
    Task<Result> UnstarAsync(int repositoryId, int currentUserId);
}
