using Domain.Enums;

namespace Application.DTOs.Request;

public class UpdateIssueRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IssuePriority Priority { get; set; }
}
