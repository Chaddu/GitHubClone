using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

public class AdminTest : BaseController
{
    [Authorize(Roles = "Admin")]
    [HttpGet("admin-test")]
    public IActionResult TestAdmin()
    {
        return Ok("You are an Admin.");
    }
}
