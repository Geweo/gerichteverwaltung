namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents an ingredient in a recipe with quantity and unit.
/// </summary>
public class RecipeIngredient : BaseEntity
{
    /// <summary>
    /// Foreign key to the recipe.
    /// </summary>
    public int RecipeId { get; set; }

    /// <summary>
    /// Navigation property to the recipe.
    /// </summary>
    public Recipe Recipe { get; set; } = null!;

    /// <summary>
    /// Name of the ingredient.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Quantity of the ingredient.
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Unit of measurement (e.g., "g", "ml", "Stück", "TL").
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Optional notes or preparation instructions for this ingredient.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Order/position of the ingredient in the recipe.
    /// </summary>
    public int Order { get; set; }
}
