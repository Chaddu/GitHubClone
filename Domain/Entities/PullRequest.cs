using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class PullRequest : SoftDeleteEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public PullRequestStatus Status { get; set; }

    public int RepositoryId { get; set; }
    public Repository Repository { get; set; } = null!;

    public int SourceBranchId { get; set; }
    public Branch SourceBranch { get; set; } = null!;

    public int TargetBranchId { get; set; }

    public Branch TargetBranch { get; set; } = null!;


    public int AuthorId { get; set; }

    public User Author { get; set; } = null!;


    public ICollection<Review> Reviews { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<PullRequest> PullRequests { get; set; } = [];


}
