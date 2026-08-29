using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class RepositoryStarConfiguration
: IEntityTypeConfiguration<RepositoryStar>
{
    public void Configure(EntityTypeBuilder<RepositoryStar> builder)
    {
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.StarredRepositories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Repository)
            .WithMany(x => x.Stars)
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
