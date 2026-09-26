using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;
public interface IRepositoryService
{
    Task<Result<RepositoryResponse>> CreateAsync(CreateRepositoryRequest request,int currentUserId);

    Task<Result<RepositoryResponse>> GetByIdAsync(int id,int currentUserId);

    Task<Result<RepositoryResponse>> GetByNameAsync(string name, int currentUserId);

    Task<Result<List<RepositoryResponse>>> GetByOwnerIdAsync(int ownerId);

    Task<Result<RepositoryResponse>> UpdateAsync(int id,UpdateRepositoryRequest request,int currentUserId);

    Task<Result> DeleteAsync(int id,int currentUserId);
    Task<Result<List<RepositoryResponse>>> GetByOrganizationIdAsync(int organizationId);

}
