namespace Application.DTOs.Request;
public class CreateCommentRequest
{
    public string Content { get; set; } = string.Empty;
    public int? IssueId { get; set; }
    public int? PullRequestId { get; set; }
}
