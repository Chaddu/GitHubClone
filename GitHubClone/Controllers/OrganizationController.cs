using Application.DTOs.Request;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrganizationController(IOrganizationService organizationService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrganizationRequest request)
    {
        var userId = GetUserId();
        var result = await organizationService.CreateAsync(request, userId);
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await organizationService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrganizations()
    {
        var userId = GetUserId();
        var result = await organizationService.GetByOwnerIdAsync(userId);
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var result = await organizationService.DeleteAsync(id, userId);
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateOrganizationRequest request)
    {
        var userId = GetUserId();
        var result = await organizationService.UpdateAsync(id, request, userId);
        return HandleResult(result);
    }
}
