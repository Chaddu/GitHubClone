using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;
public interface IOrganizationMemberService
{
    Task<Result<List<OrganizationMemberResponse>>> GetMembersAsync(int organizationId);

    Task<Result<OrganizationMemberResponse>> AddMemberAsync(int organizationId,AddOrganizationMemberRequest request,int currentUserId);

    Task<Result> UpdateRoleAsync(int organizationId,int memberId,UpdateOrganizationMemberRoleRequest request,int currentUserId);

    Task<Result> RemoveMemberAsync(int organizationId,int memberId,int currentUserId);
}
