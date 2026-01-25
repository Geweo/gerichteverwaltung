using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for ShoppingLists.
/// </summary>
public class ShoppingListFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public ShoppingList CurrentWeekShoppingList { get; private set; } = null!;

    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        var groups = Parent.GroupFixture;
        var users = Parent.UserFixture;
        var recipes = Parent.RecipeFixture;

        // PostgreSQL requires UTC DateTime, not Local
        var today = DateTime.SpecifyKind(DateTime.UtcNow.Date, DateTimeKind.Utc);
        var startDate = today.AddDays(-(int)today.DayOfWeek + 1); // Monday
        var endDate = startDate.AddDays(6); // Sunday

        CurrentWeekShoppingList = new ShoppingList
        {
            GroupId = groups.FamilieMueller.Id,
            CreatedByUserId = users.MaxMueller.Id,
            Name = "Einkaufsliste " + startDate.ToString("dd.MM.yyyy"),
            ForWeekStartDate = startDate,
            ForWeekEndDate = endDate,
            IsCompleted = false
        };
        await Context.ShoppingLists.AddAsync(CurrentWeekShoppingList, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        // Add shopping list items
        var shoppingListItems = Parent.ShoppingListItemFixture;
        var ingredients = await Context.RecipeIngredients
            .Where(ri => ri.RecipeId == recipes.SpaghettiBolognese.Id)
            .ToListAsync(cancellationToken);

        if (ingredients.Count > 0)
        {
            await shoppingListItems.AddItem(Context, CurrentWeekShoppingList, ingredients[0], cancellationToken);
            if (ingredients.Count > 1)
            {
                await shoppingListItems.AddItem(Context, CurrentWeekShoppingList, ingredients[1], cancellationToken);
            }
        }
    }
}
