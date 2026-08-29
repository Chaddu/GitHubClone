using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class PullRequestConfiguration
: IEntityTypeConfiguration<PullRequest>
{
    public void Configure(EntityTypeBuilder<PullRequest> builder)
    {
        builder
            .Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(5000);

        builder
            .HasOne(x => x.Repository)
            .WithMany(x => x.PullRequests)
            .HasForeignKey(x => x.RepositoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.SourceBranch)
            .WithMany(x => x.SourcePullRequests)
            .HasForeignKey(x => x.SourceBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.TargetBranch)
            .WithMany(x => x.TargetPullRequests)
            .HasForeignKey(x => x.TargetBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Author)
            .WithMany(x => x.AuthoredPullRequests)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
