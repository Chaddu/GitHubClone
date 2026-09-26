using Application.DTOs.Request;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class RepositoryController(IRepositoryService repositoryService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> CreateRepository([FromBody] CreateRepositoryRequest request)
    {
        var result = await repositoryService.CreateAsync(request, GetUserId());
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRepositoryById(int id)
    {
        var result = await repositoryService.GetByIdAsync(id, GetUserId());
        return HandleResult(result);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetRepositoryByName(string name)
    {
        var result = await repositoryService.GetByNameAsync(name, GetUserId());
        return HandleResult(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyRepositories()
    {
        var result = await repositoryService.GetByOwnerIdAsync(GetUserId());
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRepository(int id, [FromBody] UpdateRepositoryRequest request)
    {
        var result = await repositoryService.UpdateAsync(id, request, GetUserId());
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRepository(int id)
    {
        var result = await repositoryService.DeleteAsync(id, GetUserId());
        return HandleResult(result);
    }
}
