using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for Tag entity.
/// </summary>
public class TagEntityMap : BaseGroupMap<Tag>
{
    public override void Configure(EntityTypeBuilder<Tag> entity)
    {
        base.Configure(entity);

        entity.ToTable("Tags");

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(e => e.Category)
            .HasMaxLength(50);

        // Group relationship
        entity.HasOne(t => t.Group)
            .WithMany()
            .HasForeignKey(t => t.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint on Name + GroupId
        entity.HasIndex(t => new { t.Name, t.GroupId })
            .IsUnique();

        // RecipeTags relationship
        entity.HasMany(t => t.RecipeTags)
            .WithOne(rt => rt.Tag)
            .HasForeignKey(rt => rt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
