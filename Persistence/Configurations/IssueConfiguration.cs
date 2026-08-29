using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class IssueConfiguration
: IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
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
            .WithMany(x => x.Issues)
            .HasForeignKey(x => x.RepositoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Creator)
            .WithMany(x => x.CreatedIssues)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Assignee)
            .WithMany(x => x.AssignedIssues)
            .HasForeignKey(x => x.AssigneeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
