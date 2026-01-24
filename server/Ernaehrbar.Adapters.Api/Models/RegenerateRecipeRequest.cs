using Ernaehrbar.Parts.Ports;

namespace Ernaehrbar.Adapters.Api.Models;

/// <summary>
/// Request-Modell für die Regenerierung eines einzelnen Rezepts.
/// </summary>
public class RegenerateRecipeRequest
{
    public required string OriginalPrompt { get; init; }
    public string? NewPrompt { get; init; }
    public required MealCategory MealCategory { get; init; }
    public List<string>? ExistingTags { get; init; }
}
