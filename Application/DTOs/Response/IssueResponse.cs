using Domain.Enums;

namespace Application.DTOs.Response;
public class IssueResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IssueStatus Status { get; set; }
    public IssuePriority Priority { get; set; }
    public int RepositoryId { get; set; }
    public int CreatorId { get; set; }
    public int? AssigneeId { get; set; }
}
