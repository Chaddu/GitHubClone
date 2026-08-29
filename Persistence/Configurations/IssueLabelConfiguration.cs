using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class IssueLabelConfiguration
: IEntityTypeConfiguration<IssueLabel>
{
    public void Configure(EntityTypeBuilder<IssueLabel> builder)
    {
        builder
            .HasOne(x => x.Issue)
            .WithMany(x => x.IssueLabels)
            .HasForeignKey(x => x.IssueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Label)
            .WithMany(x => x.IssueLabels)
            .HasForeignKey(x => x.LabelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new
            {
                x.IssueId,
                x.LabelId
            })
            .IsUnique();
    }
}
