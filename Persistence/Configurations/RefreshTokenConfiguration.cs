using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .Property(x => x.Token)
            .IsRequired();

        builder
            .HasIndex(x => x.Token)
            .IsUnique();

        builder
            .Property(x => x.ExpiresAt)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.RefreshToken)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
