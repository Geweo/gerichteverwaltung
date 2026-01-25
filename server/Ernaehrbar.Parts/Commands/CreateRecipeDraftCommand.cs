using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Ports;
using MediatR;

namespace Ernaehrbar.Parts.Commands;

/// <summary>
/// Command zum Erstellen eines RecipeDraft (für Review-Prozess).
/// </summary>
public record CreateRecipeDraftCommand(
    int GroupId,
    int CreatedByUserId,
    string Name,
    RecipeSource Source,
    string? Description = null,
    string? Instructions = null,
    MealCategory? MealCategory = null,
    List<CreateRecipeDraftIngredientDto>? Ingredients = null,
    string? OriginalData = null
) : IRequest<CreateRecipeDraftResult>;

/// <summary>
/// DTO für RecipeDraft-Ingredients.
/// </summary>
public record CreateRecipeDraftIngredientDto(
    string Name,
    decimal? Quantity = null,
    string? Unit = null,
    string? Notes = null
);

/// <summary>
/// Result für CreateRecipeDraftCommand.
/// </summary>
public record CreateRecipeDraftResult(
    int Id,
    DraftStatus Status
);
