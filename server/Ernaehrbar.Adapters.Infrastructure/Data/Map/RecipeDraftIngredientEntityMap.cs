using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for RecipeDraftIngredient entity.
/// </summary>
public class RecipeDraftIngredientEntityMap : BaseMap<RecipeDraftIngredient>
{
    public override void Configure(EntityTypeBuilder<RecipeDraftIngredient> entity)
    {
        base.Configure(entity);

        entity.ToTable("RecipeDraftIngredients");

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(e => e.Unit)
            .HasMaxLength(50);

        entity.Property(e => e.Notes)
            .HasMaxLength(500);

        entity.Property(e => e.Order)
            .IsRequired();

        // RecipeDraft relationship is configured in RecipeDraftEntityMap
    }
}
