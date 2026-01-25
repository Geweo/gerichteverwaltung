using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for RecipeDraftIngredients.
/// </summary>
public class RecipeDraftIngredientFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public async Task AddIngredient(RecipeDraft draft, string name, decimal? quantity, string? unit, int order, CancellationToken cancellationToken)
    {
        var ingredient = new RecipeDraftIngredient
        {
            RecipeDraftId = draft.Id,
            Name = name,
            Quantity = quantity,
            Unit = unit,
            Order = order
        };
        await Context.RecipeDraftIngredients.AddAsync(ingredient, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    protected override Task SeedAsync(CancellationToken cancellationToken)
    {
        // RecipeDraftIngredients are created by RecipeDraftFixture
        return Task.CompletedTask;
    }
}
