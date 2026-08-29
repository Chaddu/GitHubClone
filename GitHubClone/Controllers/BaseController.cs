using Application.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GitHubClone.Controllers;
public class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result);

        return result.ErrorType switch
        {
            ErrorType.NotFound => NotFound(result),
            ErrorType.Unauthorized => Unauthorized(result),
            ErrorType.Forbidden => StatusCode(403, result),
            ErrorType.Conflict => Conflict(result),
            ErrorType.BadRequest => BadRequest(result),
            _ => StatusCode(500, result)
        };
    }

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
            return Ok(result);

        return result.ErrorType switch
        {
            ErrorType.NotFound => NotFound(result),
            ErrorType.Unauthorized => Unauthorized(result),
            ErrorType.Forbidden => StatusCode(403, result),
            ErrorType.Conflict => Conflict(result),
            ErrorType.BadRequest => BadRequest(result),
            _ => StatusCode(500, result)
        };
    }

    protected int GetUserId()
    {
        return int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}
