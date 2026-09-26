using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;
public interface IBranchService
{
    Task<Result<List<BranchResponse>>> GetByRepositoryIdAsync(int repositoryId, int currentUserId);
    Task<Result<BranchResponse>> GetByNameAsync(int repositoryId, string name, int currentUserId);
    Task<Result<BranchResponse>> CreateAsync(int repositoryId, CreateBranchRequest request, int currentUserId);
    Task<Result<BranchResponse>> UpdateAsync(int id, UpdateBranchRequest request, int currentUserId);
    Task<Result> DeleteAsync(int id, int currentUserId);
}
