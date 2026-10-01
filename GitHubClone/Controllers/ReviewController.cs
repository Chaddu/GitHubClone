using Application.DTOs.Request;
using Application.Interfaces.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReviewController(IReviewService reviewService) : BaseController
{
    [HttpGet("pull-request/{pullRequestId:int}")]
    public async Task<IActionResult> GetByPullRequestId(int pullRequestId)
    {
        var result = await reviewService.GetByPullRequestIdAsync(pullRequestId, GetUserId());

        return HandleResult(result);
    }

    [HttpGet("reviewer/{reviewerId:int}")]
    public async Task<IActionResult> GetByReviewerId(int reviewerId)
    {
        var result = await reviewService.GetByReviewerIdAsync(reviewerId);

        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await reviewService.GetByIdAsync(id, GetUserId());

        return HandleResult(result);
    }

    [HttpPost("pull-request/{pullRequestId:int}")]
    public async Task<IActionResult> Create(int pullRequestId, CreateReviewRequest request)
    {
        var result = await reviewService.CreateAsync(
            pullRequestId,
            request,
            GetUserId());

        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id,UpdateReviewRequest request)
    {
        var result = await reviewService.UpdateAsync(
            id,
            request,
            GetUserId());

        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await reviewService.DeleteAsync(
            id,
            GetUserId());

        return HandleResult(result);
    }
}
