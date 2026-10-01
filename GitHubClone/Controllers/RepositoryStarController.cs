using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RepositoryStarController(IRepositoryStarService repositoryStarService) : BaseController
{
    [HttpGet("repository/{repositoryId:int}")]
    public async Task<IActionResult> GetByRepositoryId(int repositoryId)
    {
        var result =
            await repositoryStarService.GetByRepositoryIdAsync(repositoryId, GetUserId());

        return HandleResult(result);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var result = await repositoryStarService.GetByUserIdAsync(userId);

        return HandleResult(result);
    }

    [HttpPost("repository/{repositoryId:int}")]
    public async Task<IActionResult> Star(int repositoryId)
    {
        var result = await repositoryStarService.StarAsync(repositoryId, GetUserId());

        return HandleResult(result);
    }

    [HttpDelete("repository/{repositoryId:int}")]
    public async Task<IActionResult> Unstar(int repositoryId)
    {
        var result = await repositoryStarService.UnstarAsync(repositoryId, GetUserId());

        return HandleResult(result);
    }
}
