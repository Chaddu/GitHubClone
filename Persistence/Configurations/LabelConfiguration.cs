using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class LabelConfiguration
: IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(x => x.Color)
            .HasMaxLength(7)
            .IsRequired();

        builder
            .HasOne(x => x.Repository)
            .WithMany(x => x.Labels)
            .HasForeignKey(x => x.RepositoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => new
            {
                x.RepositoryId,
                x.Name
            })
            .IsUnique();
    }
}
