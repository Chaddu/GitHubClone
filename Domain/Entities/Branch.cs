using Domain.Common;

namespace Domain.Entities;
public class Branch : AuditableEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public int RepositoryId { get; set; }
    public Repository Repository { get; set; } = null!;

    public int CreatorId { get; set; }
    public User Creator { get; set; } = null!;
    public ICollection<PullRequest> SourcePullRequests { get; set; } = [];

    public ICollection<PullRequest> TargetPullRequests { get; set; } = [];
}
