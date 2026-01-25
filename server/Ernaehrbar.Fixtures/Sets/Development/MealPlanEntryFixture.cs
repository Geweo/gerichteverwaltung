using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;
using Ernaehrbar.Parts.Ports;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for MealPlanEntries.
/// </summary>
public class MealPlanEntryFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public async Task AddEntry(ApplicationDbContext context, MealPlan mealPlan, DateTime date, MealCategory mealCategory, Recipe? recipe, int dayNumber, CancellationToken cancellationToken)
    {
        var entry = new MealPlanEntry
        {
            MealPlanId = mealPlan.Id,
            Date = date,
            MealCategory = mealCategory,
            RecipeId = recipe?.Id,
            DayNumber = dayNumber
        };
        await context.MealPlanEntries.AddAsync(entry, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    protected override Task SeedAsync(CancellationToken cancellationToken)
    {
        // MealPlanEntries are created by MealPlanFixture
        return Task.CompletedTask;
    }
}
