using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;
using Ernaehrbar.Parts.Ports;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for MealPlans.
/// </summary>
public class MealPlanFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public MealPlan CurrentWeekPlan { get; private set; } = null!;

    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        var groups = Parent.GroupFixture;
        var recipes = Parent.RecipeFixture;

        // Create a meal plan for the current week
        // PostgreSQL requires UTC DateTime, not Local
        var today = DateTime.SpecifyKind(DateTime.UtcNow.Date, DateTimeKind.Utc);
        var startDate = today.AddDays(-(int)today.DayOfWeek + 1); // Monday
        var endDate = startDate.AddDays(6); // Sunday

        CurrentWeekPlan = new MealPlan
        {
            GroupId = groups.FamilieMueller.Id,
            StartDate = startDate,
            EndDate = endDate,
            Name = "Woche " + startDate.ToString("dd.MM.yyyy"),
            Status = MealPlanStatus.Active
        };
        await Context.MealPlans.AddAsync(CurrentWeekPlan, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        // Add meal plan entries
        var mealPlanEntries = Parent.MealPlanEntryFixture;
        await mealPlanEntries.AddEntry(Context, CurrentWeekPlan, startDate, MealCategory.Breakfast, recipes.Pancakes, 1, cancellationToken);
        await mealPlanEntries.AddEntry(Context, CurrentWeekPlan, startDate, MealCategory.Dinner, recipes.SpaghettiBolognese, 1, cancellationToken);
        await mealPlanEntries.AddEntry(Context, CurrentWeekPlan, startDate.AddDays(1), MealCategory.Lunch, recipes.CaesarSalad, 2, cancellationToken);
    }
}
