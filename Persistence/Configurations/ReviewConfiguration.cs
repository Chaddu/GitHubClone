using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class ReviewConfiguration
: IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder
            .Property(x => x.Comment)
            .HasMaxLength(5000);

        builder
            .HasOne(x => x.PullRequest)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.PullRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Reviewer)
            .WithMany(x => x.SubmittedReviews)
            .HasForeignKey(x => x.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
