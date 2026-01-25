using Ernaehrbar.Parts.Domain;

namespace Ernaehrbar.Parts.Ports;

/// <summary>
/// Port for recipe repository operations (write).
/// </summary>
public interface IRecipeRepository
{
    /// <summary>
    /// Adds a new recipe.
    /// </summary>
    Task<int> AddAsync(RecipeDto recipe, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing recipe.
    /// </summary>
    Task UpdateAsync(RecipeDto recipe, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a recipe.
    /// </summary>
    Task DeleteAsync(int recipeId, CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO for recipe operations (write).
/// </summary>
public record RecipeDto(
    int? Id,
    int GroupId,
    string Name,
    RecipeSource Source,
    string? Description = null,
    string? Instructions = null,
    MealCategory? MealCategory = null,
    int? Servings = null,
    int? PreparationTimeMinutes = null,
    int? CookingTimeMinutes = null,
    int? RepeatCycleWeeks = null,
    List<RecipeIngredientDto>? Ingredients = null
);

/// <summary>
/// DTO for recipe ingredient.
/// </summary>
public record RecipeIngredientDto(
    int? Id,
    string Name,
    decimal? Quantity = null,
    string? Unit = null,
    string? Notes = null,
    int Order = 0
);
