namespace Ernaehrbar.Parts.ReadModels;

/// <summary>
/// Read-optimiertes Model für Rezepte (flach, ohne Navigation Properties).
/// Wird für Queries verwendet.
/// </summary>
public record RecipeReadModel(
    int Id,
    int GroupId,
    string Name,
    string? Description,
    string? Instructions,
    string? ImageUrl,
    string? PdfUrl,
    int? Servings,
    int? PreparationTimeMinutes,
    int? CookingTimeMinutes,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<RecipeIngredientReadModel> Ingredients,
    List<string> Tags);

/// <summary>
/// Read-optimiertes Model für Rezept-Zutaten.
/// </summary>
public record RecipeIngredientReadModel(
    int Id,
    string Name,
    decimal? Quantity,
    string? Unit,
    string? Notes,
    int Order);
