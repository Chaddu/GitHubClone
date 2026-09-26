using Application.DTOs.Request;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BranchController(IBranchService branchService) : BaseController
{
    [HttpGet("Repository/{repositoryId:int}")]
    public async Task<IActionResult> GetByRepositoryId(int repositoryId)
    {
        var result = await branchService.GetByRepositoryIdAsync(repositoryId, GetUserId());

        return HandleResult(result);
    }

    [HttpGet("repository/{repositoryId:int}/name/{name}")]
    public async Task<IActionResult> GetByName(int repositoryId, string name)
    {
        var result = await branchService.GetByNameAsync(repositoryId, name, GetUserId());
        return HandleResult(result);
    }

    [HttpPost("repository/{repositoryId:int}")]
    public async Task<IActionResult> Create(int repositoryId, [FromBody] CreateBranchRequest request)
    {
        var result = await branchService.CreateAsync(repositoryId, request, GetUserId());
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchRequest request)
    {
        var result = await branchService.UpdateAsync(id, request, GetUserId());
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await branchService.DeleteAsync(id, GetUserId());
        return HandleResult(result);
    }
}
