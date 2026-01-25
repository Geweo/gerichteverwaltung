using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Queries.Common;
using Ernaehrbar.Parts.ReadModels;

namespace Ernaehrbar.Parts.Ports;

/// <summary>
/// Port für Read-optimierte Rezept-Abfragen.
/// Separate von IRecipeStorage (Write-Operations).
/// </summary>
public interface IRecipeReadRepository
{
    /// <summary>
    /// Ruft ein Rezept anhand der ID ab.
    /// </summary>
    Task<RecipeReadModel?> GetByIdAsync(int recipeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ruft eine paginierte Liste von Rezepten mit optionaler Filterung und Sortierung ab.
    /// </summary>
    Task<PaginatedResult<RecipeReadModel>> GetRecipesAsync(
        int groupId,
        int page,
        int pageSize,
        string? searchTerm = null,
        MealCategory? mealCategory = null,
        RecipeSource? source = null,
        bool? favorites = null,
        List<int>? tagIds = null,
        RecipeListSorting sortBy = RecipeListSorting.Name,
        SortDirectionEnum sortDirection = SortDirectionEnum.Asc,
        CancellationToken cancellationToken = default);
}
