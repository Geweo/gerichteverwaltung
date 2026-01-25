namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents a single item in a shopping list.
/// </summary>
public class ShoppingListItem : BaseEntity
{
    /// <summary>
    /// Foreign key to the shopping list.
    /// </summary>
    public int ShoppingListId { get; set; }

    /// <summary>
    /// Navigation property to the shopping list.
    /// </summary>
    public ShoppingList ShoppingList { get; set; } = null!;

    /// <summary>
    /// Foreign key to the recipe ingredient this item references (nullable).
    /// </summary>
    public int? RecipeIngredientId { get; set; }

    /// <summary>
    /// Navigation property to the recipe ingredient.
    /// </summary>
    public RecipeIngredient? RecipeIngredient { get; set; }

    /// <summary>
    /// Ingredient name (denormalized for flexibility).
    /// </summary>
    public required string IngredientName { get; set; }

    /// <summary>
    /// Aggregated quantity.
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Unit of measurement.
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Whether this item is checked off.
    /// </summary>
    public bool IsChecked { get; set; }

    /// <summary>
    /// Order/position in the shopping list.
    /// </summary>
    public int Order { get; set; }
}
