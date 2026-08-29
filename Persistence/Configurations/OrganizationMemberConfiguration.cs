using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class OrganizationMemberConfiguration
    : IEntityTypeConfiguration<OrganizationMember>
{
    public void Configure(EntityTypeBuilder<OrganizationMember> builder)
    {
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.OrganizationMemberships)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Organization)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => new
            {
                x.UserId,
                x.OrganizationId
            })
            .IsUnique();
    }
}
