using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;
public class Issue : SoftDeleteEntity<int>
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;


    public IssueStatus Status { get; set; }

    public IssuePriority Priority { get; set; }


    public int RepositoryId { get; set; }

    public Repository Repository { get; set; } = null!;


    public int CreatorId { get; set; }

    public User Creator { get; set; } = null!;


    public int? AssigneeId { get; set; }

    public User? Assignee { get; set; }


    public ICollection<Comment> Comments { get; set; } = [];

    public ICollection<IssueLabel> IssueLabels { get; set; } = [];
}
