using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for RecipeDraft entity.
/// </summary>
public class RecipeDraftEntityMap : BaseGroupMap<RecipeDraft>
{
    public override void Configure(EntityTypeBuilder<RecipeDraft> entity)
    {
        base.Configure(entity);

        entity.ToTable("RecipeDrafts");

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(e => e.Description)
            .HasMaxLength(1000);

        entity.Property(e => e.Instructions)
            .HasMaxLength(5000);

        // Group relationship
        entity.HasOne(rd => rd.Group)
            .WithMany(g => g.RecipeDrafts)
            .HasForeignKey(rd => rd.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // CreatedByUser relationship
        entity.HasOne(rd => rd.CreatedByUser)
            .WithMany(u => u.CreatedRecipeDrafts)
            .HasForeignKey(rd => rd.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ReviewedByUser relationship (nullable)
        entity.HasOne(rd => rd.ReviewedByUser)
            .WithMany(u => u.ReviewedRecipeDrafts)
            .HasForeignKey(rd => rd.ReviewedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ingredients relationship
        entity.HasMany(rd => rd.Ingredients)
            .WithOne(rdi => rdi.RecipeDraft)
            .HasForeignKey(rdi => rdi.RecipeDraftId)
            .OnDelete(DeleteBehavior.Cascade);

        // Files relationship
        entity.HasMany(rd => rd.Files)
            .WithOne(f => f.RecipeDraft)
            .HasForeignKey(f => f.RecipeDraftId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
