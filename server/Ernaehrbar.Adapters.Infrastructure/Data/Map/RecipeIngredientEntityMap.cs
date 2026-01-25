using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for RecipeIngredient entity.
/// </summary>
public class RecipeIngredientEntityMap : BaseMap<RecipeIngredient>
{
    public override void Configure(EntityTypeBuilder<RecipeIngredient> entity)
    {
        base.Configure(entity);

        entity.ToTable("RecipeIngredients");

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(e => e.Unit)
            .HasMaxLength(50);

        entity.Property(e => e.Notes)
            .HasMaxLength(500);

        entity.Property(e => e.Order)
            .IsRequired();

        // Recipe relationship is configured in RecipeEntityMap
    }
}
