using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for MealPlanEntry entity.
/// </summary>
public class MealPlanEntryEntityMap : BaseMap<MealPlanEntry>
{
    public override void Configure(EntityTypeBuilder<MealPlanEntry> entity)
    {
        base.Configure(entity);

        entity.ToTable("MealPlanEntries");

        entity.Property(e => e.Date)
            .IsRequired();

        entity.Property(e => e.MealCategory)
            .IsRequired();

        // MealPlan relationship
        entity.HasOne(mpe => mpe.MealPlan)
            .WithMany(mp => mp.Entries)
            .HasForeignKey(mpe => mpe.MealPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        // Recipe relationship (nullable)
        entity.HasOne(mpe => mpe.Recipe)
            .WithMany(r => r.MealPlanEntries)
            .HasForeignKey(mpe => mpe.RecipeId)
            .OnDelete(DeleteBehavior.SetNull);

        // Index on Date and MealCategory for faster queries
        entity.HasIndex(mpe => new { mpe.MealPlanId, mpe.Date, mpe.MealCategory });
    }
}
