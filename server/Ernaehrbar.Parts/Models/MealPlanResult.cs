using Ernaehrbar.Parts.Ports;

namespace Ernaehrbar.Parts.Models;

/// <summary>
/// Ergebnis der Wochenplan-Generierung.
/// </summary>
public class MealPlanResult
{
    public required string Prompt { get; init; }
    public required List<string> Tags { get; init; }
    public required List<GeneratedRecipe> Recipes { get; init; }
    public required List<MealCategory> MealCategories { get; init; }
    public int NumberOfDays { get; init; }
    public DateTime GeneratedAt { get; init; }
}
