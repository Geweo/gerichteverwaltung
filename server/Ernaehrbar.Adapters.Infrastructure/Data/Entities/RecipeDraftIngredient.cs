namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Ingredient of a recipe draft.
/// </summary>
public class RecipeDraftIngredient : BaseEntity
{
    /// <summary>
    /// Foreign key to the recipe draft.
    /// </summary>
    public int RecipeDraftId { get; set; }

    /// <summary>
    /// Navigation property to the recipe draft.
    /// </summary>
    public RecipeDraft RecipeDraft { get; set; } = null!;

    /// <summary>
    /// Name of the ingredient.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Quantity of the ingredient.
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Unit of measurement (e.g., "g", "ml", "Stück").
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Notes about this ingredient.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Order/position in the ingredient list.
    /// </summary>
    public int Order { get; set; }
}
