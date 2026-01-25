using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for RecipeIngredients.
/// </summary>
public class RecipeIngredientFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public async Task AddIngredient(ApplicationDbContext context, Recipe recipe, string name, decimal? quantity, string? unit, int order, CancellationToken cancellationToken)
    {
        var ingredient = new RecipeIngredient
        {
            RecipeId = recipe.Id,
            Name = name,
            Quantity = quantity,
            Unit = unit,
            Order = order
        };
        await context.RecipeIngredients.AddAsync(ingredient, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    protected override Task SeedAsync(CancellationToken cancellationToken)
    {
        // RecipeIngredients are created by RecipeFixture
        return Task.CompletedTask;
    }
}
