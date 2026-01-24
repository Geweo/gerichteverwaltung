using Ernaehrbar.Parts.Ports;

namespace Ernaehrbar.Adapters.Api.Models;

/// <summary>
/// Request-Modell für die Rezeptgenerierung.
/// </summary>
public class GenerateRecipesRequest
{
    public required string Prompt { get; init; }
    public required List<MealCategory> MealCategories { get; init; }
    public int NumberOfDays { get; init; } = 7;
}
