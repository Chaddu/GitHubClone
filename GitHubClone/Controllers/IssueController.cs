using Application.DTOs.Request;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class IssueController(IIssueService issueService) : BaseController
{
    [HttpGet("Repository/{repositoryId:int}")]
    public async Task<IActionResult> GetByRepositoryId(int repositoryId)
    {
        var issues = await issueService.GetByRepositoryIdAsync(repositoryId, GetUserId());
        return HandleResult(issues);
    }

    [HttpGet("Repository/{repositoryId:int}/Open")]
    public async Task<IActionResult> GetOpenIssues(int repositoryId)
    {
        var issues = await issueService.GetOpenIssuesAsync(repositoryId, GetUserId());
        return HandleResult(issues);
    }
    [HttpGet("Author/{authorId:int}")]
    public async Task<IActionResult> GetByAuthorId(int authorId)
    {
        var issues = await issueService.GetByAuthorIdAsync(authorId);
        return Ok(issues);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var issue = await issueService.GetByIdAsync(id, GetUserId());
        
        return HandleResult(issue);
    }
    [HttpPost("repository/{repositoryId:int}")]
    public async Task<IActionResult> Create(int repositoryId, [FromBody] CreateIssueRequest request)
    {
        var issue = await issueService.CreateAsync(repositoryId, request, GetUserId());
        return HandleResult(issue);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateIssueRequest request)
    {
        var issue = await issueService.UpdateAsync(id, request, GetUserId());
        return HandleResult(issue);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await issueService.DeleteAsync(id, GetUserId());
        return NoContent();
    }
}
