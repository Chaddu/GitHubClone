using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class RepositoryMemberConfiguration
: IEntityTypeConfiguration<RepositoryMember>
{
    public void Configure(EntityTypeBuilder<RepositoryMember> builder)
    {
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.RepositoryMemberships)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Repository)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.RepositoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => new
            {
                x.UserId,
                x.RepositoryId
            })
            .IsUnique();
    }
}
