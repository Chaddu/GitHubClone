using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class RepositoryConfiguration : IEntityTypeConfiguration<Repository>
{
    public void Configure(EntityTypeBuilder<Repository> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(500);

        builder
            .HasIndex(x => new
            {
                x.OwnerId,
                x.Name
            })
            .IsUnique();

        builder
            .HasOne(x => x.Owner)
            .WithMany(x => x.OwnedRepositories)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Organization)
            .WithMany(x => x.Repositories)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
