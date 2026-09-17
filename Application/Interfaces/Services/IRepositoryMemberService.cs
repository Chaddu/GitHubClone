using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;
public interface IRepositoryMemberService
{
    Task<Result<List<RepositoryMemberResponse>>> GetMembersAsync(int repositoryId);
    Task<Result<RepositoryMemberResponse>> AddMemberAsync(int repositoryId, AddRepositoryMemberRequest request, int currentUserId);
    Task<Result> UpdatePermissionAsync(int repositoryId, int memberId, UpdateRepositoryMemberPermissionRequest request, int currentUserId);
    Task<Result> RemoveMemberAsync(int repositoryId, int memberId, int currentUserId);
}
