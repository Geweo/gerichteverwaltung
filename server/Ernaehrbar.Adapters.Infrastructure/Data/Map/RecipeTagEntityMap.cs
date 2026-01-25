using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for RecipeTag (join table) entity.
/// </summary>
public class RecipeTagEntityMap : BaseMap<RecipeTag>
{
    public override void Configure(EntityTypeBuilder<RecipeTag> entity)
    {
        base.Configure(entity);

        entity.ToTable("RecipeTags");

        // Composite unique constraint
        entity.HasIndex(rt => new { rt.RecipeId, rt.TagId })
            .IsUnique();

        // Relationships are configured in RecipeEntityMap and TagEntityMap
    }
}
