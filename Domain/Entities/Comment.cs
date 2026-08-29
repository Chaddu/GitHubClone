using Domain.Common;

namespace Domain.Entities;
public class Comment : SoftDeleteEntity<int>
{
    public string Content { get; set; } = string.Empty;


    public int AuthorId { get; set; }

    public User Author { get; set; } = null!;


    public int? IssueId { get; set; }

    public Issue? Issue { get; set; }


    public int? PullRequestId { get; set; }

    public PullRequest? PullRequest { get; set; }
}

