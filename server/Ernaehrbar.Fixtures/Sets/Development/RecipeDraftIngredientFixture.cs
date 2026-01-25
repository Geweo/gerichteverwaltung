using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for RecipeDraftIngredients.
/// </summary>
public class RecipeDraftIngredientFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public async Task AddIngredient(ApplicationDbContext context, RecipeDraft draft, string name, decimal? quantity, string? unit, int order, CancellationToken cancellationToken)
    {
        var ingredient = new RecipeDraftIngredient
        {
            RecipeDraftId = draft.Id,
            Name = name,
            Quantity = quantity,
            Unit = unit,
            Order = order
        };
        await context.RecipeDraftIngredients.AddAsync(ingredient, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    protected override Task SeedAsync(CancellationToken cancellationToken)
    {
        // RecipeDraftIngredients are created by RecipeDraftFixture
        return Task.CompletedTask;
    }
}
