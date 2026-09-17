namespace Application.DTOs.Response;
public class CommentResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public int? IssueId { get; set; }
    public int? PullRequestId { get; set; }
}
