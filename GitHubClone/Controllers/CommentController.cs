using Application.DTOs.Request;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CommentController(ICommentService commentService) : BaseController
{
    [HttpGet("author/{authorId:int}")]
    public async Task<IActionResult> GetByAuthorId(int authorId)
    {
        var result = await commentService.GetByAuthorIdAsync(authorId);

        return HandleResult(result);
    }

    [HttpGet("issue/{issueId:int}")]
    public async Task<IActionResult> GetByIssueId(int issueId)
    {
        var result = await commentService.GetByIssueIdAsync(issueId);

        return HandleResult(result);
    }

    [HttpGet("pull-request/{pullRequestId:int}")]
    public async Task<IActionResult> GetByPullRequestId(int pullRequestId)
    {
        var result = await commentService.GetByPullRequestIdAsync(pullRequestId);

        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await commentService.GetByIdAsync(id);

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
    {
        var result = await commentService.CreateAsync(request, GetUserId());

        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCommentRequest request)
    {
        var result = await commentService.UpdateAsync(id,request, GetUserId());

        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await commentService.DeleteAsync(id,GetUserId());

        return HandleResult(result);
    }
}
