namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents a meal plan (weekly plan) for a group.
/// </summary>
public class MealPlan : BaseGroupEntity
{
    /// <summary>
    /// Start date of the meal plan (typically Monday of the week).
    /// </summary>
    public required DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the meal plan (typically Sunday of the week).
    /// </summary>
    public required DateTime EndDate { get; set; }

    /// <summary>
    /// Optional name/description of the meal plan.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Prompt that was used to generate this meal plan (if generated).
    /// </summary>
    public string? GenerationPrompt { get; set; }

    /// <summary>
    /// Navigation property to meal plan entries.
    /// </summary>
    public ICollection<MealPlanEntry> Entries { get; set; } = new List<MealPlanEntry>();
}
