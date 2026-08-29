using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;
public class Repository : SoftDeleteEntity<int>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public RepositoryVisibility Visibility { get; set; }

    public int? OwnerId { get; set; }
    public User? Owner { get; set; }

    public int? OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public ICollection<RepositoryMember> Members { get; set; } = [];

    public ICollection<Branch> Branches { get; set; } = [];

    public ICollection<Issue> Issues { get; set; } = [];


    public ICollection<PullRequest> PullRequests { get; set; } = [];
    public ICollection<Activity> Activities { get; set; } = [];
    public ICollection<RepositoryStar> Stars { get; set; } = [];
    public ICollection<Label> Labels { get; set; } = [];
}
