using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class BranchConfiguration
: IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .HasOne(x => x.Repository)
            .WithMany(x => x.Branches)
            .HasForeignKey(x => x.RepositoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Creator)
            .WithMany(x => x.CreatedBranches)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new
            {
                x.RepositoryId,
                x.Name
            })
            .IsUnique();
    }
}
