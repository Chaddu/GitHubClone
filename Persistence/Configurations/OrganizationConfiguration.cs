using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class OrganizationConfiguration
: IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(500);

        builder
            .HasIndex(x => x.Name)
            .IsUnique();

        builder
            .HasOne(x => x.Owner)
            .WithMany(x => x.OwnedOrganization)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Members)
            .WithOne(x => x.Organization)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Repositories)
            .WithOne(x => x.Organization)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
