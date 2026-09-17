using Application.DTOs.Request;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LabelController(ILabelService labelService) : BaseController
{
    [HttpGet("repository/{repositoryId:int}")]
    public async Task<IActionResult> GetLabelsByRepositoryId(int repositoryId)
    {
        var result = await labelService.GetByRepositoryIdAsync(repositoryId);
        return HandleResult(result);
    }

    [HttpGet("repository/{repositoryId:int}/name/{name}")]
   
    public async Task<IActionResult> GetByName(int repositoryId, string name)
    {
        var result = await labelService.GetByNameAsync(repositoryId,name);
        return HandleResult(result);
    }

    [HttpPost("repository/{repositoryId:int}")]
    public async Task<IActionResult> CreateLabel(int repositoryId, [FromBody] CreateLabelRequest request)
    {
        var result = await labelService.CreateAsync(repositoryId, request, GetUserId());
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateLabel(int id, [FromBody] UpdateLabelRequest request)
    {
        var result = await labelService.UpdateAsync(id, request);
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await labelService.DeleteAsync(id);
        return HandleResult(result);
    }


}
