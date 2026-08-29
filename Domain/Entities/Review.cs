using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;
public class Review : AuditableEntity<int>
{
    public ReviewState State { get; set; }

    public string? Comment { get; set; }


    public int PullRequestId { get; set; }

    public PullRequest PullRequest { get; set; } = null!;


    public int ReviewerId { get; set; }

    public User Reviewer { get; set; } = null!;
}
