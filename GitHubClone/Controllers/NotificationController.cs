using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubClone.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationController(INotificationService notificationService)  : BaseController
{
    [HttpGet("my")]
    public async Task<IActionResult> GetMyNotifications()
    {
        var result =
            await notificationService.GetMyNotificationsAsync(GetUserId());

        return HandleResult(result);
    }

    [HttpGet("my/unread")]
    public async Task<IActionResult> GetMyUnreadNotifications()
    {
        var result =
            await notificationService.GetMyUnreadNotificationsAsync(GetUserId());

        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result =
            await notificationService.GetByIdAsync(id, GetUserId());

        return HandleResult(result);
    }

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result =
            await notificationService.MarkAsReadAsync(id, GetUserId());

        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await notificationService.DeleteAsync(id, GetUserId());

        return HandleResult(result);
    }
}
