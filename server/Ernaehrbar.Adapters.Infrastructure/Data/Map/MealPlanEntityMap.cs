using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for MealPlan entity.
/// </summary>
public class MealPlanEntityMap : BaseGroupMap<MealPlan>
{
    public override void Configure(EntityTypeBuilder<MealPlan> entity)
    {
        base.Configure(entity);

        entity.ToTable("MealPlans");

        entity.Property(e => e.Name)
            .HasMaxLength(200);

        // Group relationship
        entity.HasOne(mp => mp.Group)
            .WithMany(g => g.MealPlans)
            .HasForeignKey(mp => mp.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Entries relationship
        entity.HasMany(mp => mp.Entries)
            .WithOne(mpe => mpe.MealPlan)
            .HasForeignKey(mpe => mpe.MealPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
