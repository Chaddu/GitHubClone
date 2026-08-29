using Application.DTOs.Request;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrganizationMemberController(IOrganizationMemberService organizationMember) : BaseController
{
    [HttpPost("{organizationId:int}/members")]
    public async Task<IActionResult> AddMember(int organizationId, AddOrganizationMemberRequest request)
    {
        var userId = GetUserId();
        
        var result = await organizationMember.AddMemberAsync(organizationId, request, userId);

        return HandleResult(result);
    }

    [HttpGet("{organizationId:int}/members")]
    public async Task<IActionResult> GetMembers(int organizationId)
    {
        var result = await organizationMember.GetMembersAsync(organizationId);
        return HandleResult(result);
    }

    [HttpPut("{organizationId:int}/{memberId:int}")]
    public async Task<IActionResult> UpdateRole(int organizationId, int memberId, UpdateOrganizationMemberRoleRequest request)
    {
        var userId = GetUserId();
        var result = await organizationMember.UpdateRoleAsync(organizationId, memberId, request, userId);
        return HandleResult(result);
    }

    [HttpDelete("{organizationId:int}/{memberId:int}")]
    public async Task<IActionResult> RemoveMember(int organizationId, int memberId)
    {
        var userId = GetUserId();
        var result = await organizationMember.RemoveMemberAsync(organizationId, memberId, userId);
        return HandleResult(result);
    }
    
}
