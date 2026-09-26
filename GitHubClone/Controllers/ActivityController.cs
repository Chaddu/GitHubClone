using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController(IActivityService activityService) : BaseController
{
    [HttpGet("repository/{repositoryId:int}")]
    public async Task<IActionResult> GetByRepositoryId(int repositoryId)
    {
        var result = await activityService.GetByRepositoryIdAsync(repositoryId, GetUserId());

        return HandleResult(result);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var result = await activityService.GetByUserIdAsync(userId, GetUserId());

        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await activityService.GetByIdAsync(id, GetUserId());

        return HandleResult(result);
    }
}
