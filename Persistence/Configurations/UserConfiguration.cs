using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
           .Property(x => x.Username)
           .HasMaxLength(50)
           .IsRequired();


        builder
            .Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();


        builder
            .Property(x => x.PasswordHash)
            .IsRequired();


        builder
            .HasIndex(x => x.Email)
            .IsUnique();


        builder
            .HasIndex(x => x.Username)
            .IsUnique();


        builder.Property(x => x.Role)
            .HasConversion<string>()
            .HasDefaultValue(UserRole.User)
            .IsRequired();
    }
}
