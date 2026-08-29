using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Persistence.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder
            .Property(x => x.Action)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Activities)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Repository)
            .WithMany(x => x.Activities)
            .HasForeignKey(x => x.RepositoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => x.RepositoryId);

    }
}