using Domain.Common;
using Domain.Enums;



namespace Domain.Entities;
public class User : AuditableEntity<int>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }  = UserRole.User;

    public ICollection<RefreshToken> RefreshToken { get; set; } = [];
    public ICollection<Organization> OwnedOrganization { get; set; } = [];

    public ICollection<OrganizationMember> OrganizationMemberships { get; set; } = [];

    public ICollection<Repository> OwnedRepositories { get; set; } = [];

    public ICollection<RepositoryMember> RepositoryMemberships { get; set; } = [];

    public ICollection<Issue> AssignedIssues { get; set; } = [];
    public ICollection<Issue> CreatedIssues { get; set; } = [];

    public ICollection<PullRequest> AuthoredPullRequests { get; set; } = [];

    public ICollection<Review> SubmittedReviews { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];

    public ICollection<Notification> Notifications { get; set; } = [];

    public ICollection<Activity> Activities { get; set; } = [];


    public ICollection<RepositoryStar> StarredRepositories { get; set; } = [];
    public ICollection<Branch> CreatedBranches { get; set; } = [];


}
