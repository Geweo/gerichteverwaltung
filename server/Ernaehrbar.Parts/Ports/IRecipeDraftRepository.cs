using Ernaehrbar.Parts.Domain;

namespace Ernaehrbar.Parts.Ports;

/// <summary>
/// Port for recipe draft repository operations (write).
/// </summary>
public interface IRecipeDraftRepository
{
    /// <summary>
    /// Adds a new recipe draft.
    /// </summary>
    Task<int> AddAsync(RecipeDraftDto draft, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a recipe draft by ID.
    /// </summary>
    Task<RecipeDraftDto?> GetByIdAsync(int draftId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a recipe draft.
    /// </summary>
    Task UpdateAsync(RecipeDraftDto draft, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a recipe draft.
    /// </summary>
    Task DeleteAsync(int draftId, CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO for recipe draft operations (write).
/// </summary>
public record RecipeDraftDto(
    int? Id,
    int GroupId,
    int CreatedByUserId,
    string Name,
    RecipeSource Source,
    DraftStatus Status,
    string? Description = null,
    string? Instructions = null,
    MealCategory? MealCategory = null,
    string? OriginalData = null,
    int? ReviewedByUserId = null,
    DateTime? ReviewedAt = null,
    List<RecipeDraftIngredientDto>? Ingredients = null
);

/// <summary>
/// DTO for recipe draft ingredient.
/// </summary>
public record RecipeDraftIngredientDto(
    int? Id,
    string Name,
    decimal? Quantity = null,
    string? Unit = null,
    string? Notes = null,
    int Order = 0
);
