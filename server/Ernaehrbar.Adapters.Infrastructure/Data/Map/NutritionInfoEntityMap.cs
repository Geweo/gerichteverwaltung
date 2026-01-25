using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for NutritionInfo entity.
/// </summary>
public class NutritionInfoEntityMap : BaseMap<NutritionInfo>
{
    public override void Configure(EntityTypeBuilder<NutritionInfo> entity)
    {
        base.Configure(entity);

        entity.ToTable("NutritionInfos");

        // Recipe relationship (one-to-one)
        // Note: The relationship is configured in RecipeEntityMap
        // This ensures the foreign key is on NutritionInfo side
    }
}
