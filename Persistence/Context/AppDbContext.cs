using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { 
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Organization> Organizations => Set<Organization>();

    public DbSet<OrganizationMember> OrganizationMembers => Set<OrganizationMember>();

    public DbSet<Repository> Repositories => Set<Repository>();

    public DbSet<RepositoryMember> RepositoryMembers => Set<RepositoryMember>();

    public DbSet<RepositoryStar> RepositoryStars => Set<RepositoryStar>();

    public DbSet<Branch> Branches => Set<Branch>();

    public DbSet<Issue> Issues => Set<Issue>();

    public DbSet<Label> Labels => Set<Label>();

    public DbSet<IssueLabel> IssueLabels => Set<IssueLabel>();

    public DbSet<PullRequest> PullRequests => Set<PullRequest>();

    public DbSet<Review> Reviews => Set<Review>();

    public DbSet<Comment> Comments => Set<Comment>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}
