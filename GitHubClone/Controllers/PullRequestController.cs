using Application.DTOs.Request;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PullRequestController(IPullRequestService pullRequestService) : BaseController
{
    [HttpGet("repository/{repositoryId:int}")]
    public async Task<IActionResult> GetByRepositoryId(int repositoryId)
    {
        var result = await pullRequestService.GetByRepositoryIdAsync(repositoryId);
        return HandleResult(result);
    }

    [HttpGet("author/{authorId:int}")]
    public async Task<IActionResult> GetByAuthorId(int authorId)
    {
        var result = await pullRequestService.GetByAuthorIdAsync(authorId);
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await pullRequestService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpPost("repository/{repositoryId:int}")]
    public async Task<IActionResult> Create(int repositoryId, [FromBody] CreatePullRequestRequest request)
    {
        var result = await pullRequestService.CreateAsync(repositoryId, request, GetUserId());
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePullRequestRequest request)
    {
        var result = await pullRequestService.UpdateAsync(id, request, GetUserId());
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await pullRequestService.DeleteAsync(id, GetUserId());
        return HandleResult(result);
    }
}

