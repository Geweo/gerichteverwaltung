using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for ShoppingListItems.
/// </summary>
public class ShoppingListItemFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public async Task AddItem(ShoppingList shoppingList, RecipeIngredient? recipeIngredient, CancellationToken cancellationToken)
    {
        var item = new ShoppingListItem
        {
            ShoppingListId = shoppingList.Id,
            IngredientName = recipeIngredient?.Name ?? "Einkaufsliste Item",
            Quantity = recipeIngredient?.Quantity,
            Unit = recipeIngredient?.Unit,
            IsChecked = false,
            RecipeIngredientId = recipeIngredient?.Id,
            Order = 1
        };
        await Context.ShoppingListItems.AddAsync(item, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    protected override Task SeedAsync(CancellationToken cancellationToken)
    {
        // ShoppingListItems are created by ShoppingListFixture
        return Task.CompletedTask;
    }
}
