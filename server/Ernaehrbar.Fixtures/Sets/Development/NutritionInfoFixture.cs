using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for NutritionInfo.
/// </summary>
public class NutritionInfoFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public async Task AddNutritionInfo(
        ApplicationDbContext context,
        Recipe recipe,
        decimal? calories = null,
        decimal? protein = null,
        decimal? carbohydrates = null,
        decimal? fat = null,
        decimal? fiber = null,
        decimal? sugar = null,
        decimal? sodium = null,
        CancellationToken cancellationToken = default)
    {
        var nutritionInfo = new NutritionInfo
        {
            RecipeId = recipe.Id,
            Calories = calories,
            Protein = protein,
            Carbohydrates = carbohydrates,
            Fat = fat,
            Fiber = fiber,
            Sugar = sugar,
            Sodium = sodium
        };
        await context.NutritionInfos.AddAsync(nutritionInfo, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    protected override Task SeedAsync(CancellationToken cancellationToken)
    {
        // NutritionInfo is created by RecipeFixture
        return Task.CompletedTask;
    }
}
