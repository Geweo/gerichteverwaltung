using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for ShoppingListItems.
/// </summary>
public class ShoppingListItemFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public async Task AddItem(ApplicationDbContext context, ShoppingList shoppingList, RecipeIngredient? recipeIngredient, CancellationToken cancellationToken)
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
        await context.ShoppingListItems.AddAsync(item, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    protected override Task SeedAsync(CancellationToken cancellationToken)
    {
        // ShoppingListItems are created by ShoppingListFixture
        return Task.CompletedTask;
    }
}
