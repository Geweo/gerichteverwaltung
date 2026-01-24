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
    /// Ruft eine Liste von Rezepten mit optionaler Filterung ab.
    /// </summary>
    Task<List<RecipeReadModel>> GetRecipesAsync(
        int groupId,
        List<int>? tagIds = null,
        string? searchTerm = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default);
}
