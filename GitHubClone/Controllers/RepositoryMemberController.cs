using Application.DTOs.Request;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RepositoryMemberController(IRepositoryMemberService repoMemberService) : BaseController
{
    [HttpPost("{repositoryId:int}")]
    public async Task<IActionResult> AddMember(int repositoryId, [FromBody] AddRepositoryMemberRequest request)
    {
        var result = await repoMemberService.AddMemberAsync(repositoryId, request, GetUserId());
        return HandleResult(result);
    }

    [HttpGet("{repositoryId:int}")]

    public async Task<IActionResult> GetMembers(int repositoryId)
    {
        var result = await repoMemberService.GetMembersAsync(repositoryId);
        return HandleResult(result);
    }

    [HttpPut("{repositoryId:int}/{memberId:int}")]
    public async Task<IActionResult> UpdatePermission(int repositoryId, int memberId, [FromBody] UpdateRepositoryMemberPermissionRequest request)
    {
        var result = await repoMemberService.UpdatePermissionAsync(repositoryId, memberId, request, GetUserId());
        return HandleResult(result);
    }
    [HttpDelete("{repositoryId:int}/{memberId:int}")]
    public async Task<IActionResult> RemoveMember(int repositoryId, int memberId)
    {
        var result = await repoMemberService.RemoveMemberAsync(repositoryId, memberId, GetUserId());
        return HandleResult(result);
    }
}
