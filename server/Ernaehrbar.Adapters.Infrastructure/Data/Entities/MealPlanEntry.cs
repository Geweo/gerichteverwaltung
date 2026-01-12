using Ernaehrbar.Parts.Ports;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents a single meal entry in a meal plan (one meal on one day).
/// </summary>
public class MealPlanEntry : BaseEntity
{
    /// <summary>
    /// Foreign key to the meal plan.
    /// </summary>
    public int MealPlanId { get; set; }

    /// <summary>
    /// Navigation property to the meal plan.
    /// </summary>
    public MealPlan MealPlan { get; set; } = null!;

    /// <summary>
    /// Date of this meal entry.
    /// </summary>
    public required DateTime Date { get; set; }

    /// <summary>
    /// Meal category (Breakfast, Lunch, Dinner).
    /// Uses MealCategory enum from Ernaehrbar.Parts.Ports.
    /// </summary>
    public MealCategory MealCategory { get; set; }

    /// <summary>
    /// Foreign key to the recipe (nullable, can be unassigned).
    /// </summary>
    public int? RecipeId { get; set; }

    /// <summary>
    /// Navigation property to the recipe.
    /// </summary>
    public Recipe? Recipe { get; set; }

    /// <summary>
    /// Optional custom meal name if no recipe is assigned.
    /// </summary>
    public string? CustomMealName { get; set; }

    /// <summary>
    /// Day number within the meal plan (1-7 for weekly plans).
    /// </summary>
    public int DayNumber { get; set; }
}
