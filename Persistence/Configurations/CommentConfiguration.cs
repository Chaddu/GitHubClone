using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class CommentConfiguration
: IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
           .Property(x => x.Content)
           .HasMaxLength(5000)
           .IsRequired();

        builder
            .HasOne(x => x.Author)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Issue)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.IssueId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.PullRequest)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.PullRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .ToTable(table =>
                table.HasCheckConstraint(
                    "CK_Comment_Parent",
                    "(IssueId IS NOT NULL AND PullRequestId IS NULL) OR " +
                    "(IssueId IS NULL AND PullRequestId IS NOT NULL)"));
    }
}
