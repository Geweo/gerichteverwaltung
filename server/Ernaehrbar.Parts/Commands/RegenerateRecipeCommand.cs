using Ernaehrbar.Parts.Ports;
using MediatR;

namespace Ernaehrbar.Parts.Commands;

/// <summary>
/// Command zur Regenerierung eines einzelnen Rezepts.
/// </summary>
public record RegenerateRecipeCommand(
    string OriginalPrompt,
    string? NewPrompt,
    MealCategory MealCategory,
    List<string> ExistingTags) : IRequest<GeneratedRecipe>;
