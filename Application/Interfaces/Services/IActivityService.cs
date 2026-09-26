using Application.Common;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;
public interface IActivityService
{
    Task<Result<List<ActivityResponse>>> GetByRepositoryIdAsync(int repositoryId, int currentUserId);
    Task<Result<List<ActivityResponse>>> GetByUserIdAsync(int userId, int currentUserId);
    Task<Result<ActivityResponse>> GetByIdAsync(int id, int currentUserId);
}
