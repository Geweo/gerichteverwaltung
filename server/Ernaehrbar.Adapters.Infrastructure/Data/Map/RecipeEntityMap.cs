using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for Recipe entity.
/// </summary>
public class RecipeEntityMap : BaseGroupMap<Recipe>
{
    public override void Configure(EntityTypeBuilder<Recipe> entity)
    {
        base.Configure(entity);

        entity.ToTable("Recipes");

        // Group relationship
        entity.HasOne(r => r.Group)
            .WithMany(g => g.Recipes)
            .HasForeignKey(r => r.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ingredients relationship
        entity.HasMany(r => r.Ingredients)
            .WithOne(ri => ri.Recipe)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // RecipeTags relationship
        entity.HasMany(r => r.RecipeTags)
            .WithOne(rt => rt.Recipe)
            .HasForeignKey(rt => rt.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ratings relationship
        entity.HasMany(r => r.Ratings)
            .WithOne(rr => rr.Recipe)
            .HasForeignKey(rr => rr.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // NutritionInfo relationship (one-to-one)
        entity.HasOne(r => r.NutritionInfo)
            .WithOne(ni => ni.Recipe)
            .HasForeignKey<NutritionInfo>(ni => ni.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // MealPlanEntries relationship
        entity.HasMany(r => r.MealPlanEntries)
            .WithOne(mpe => mpe.Recipe)
            .HasForeignKey(mpe => mpe.RecipeId)
            .OnDelete(DeleteBehavior.SetNull);

        // Files relationship
        entity.HasMany(r => r.Files)
            .WithOne(f => f.Recipe)
            .HasForeignKey(f => f.RecipeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
