namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents a shopping list for a group/week.
/// </summary>
public class ShoppingList : BaseGroupEntity
{
    /// <summary>
    /// Foreign key to the user who created this shopping list.
    /// </summary>
    public int CreatedByUserId { get; set; }

    /// <summary>
    /// Navigation property to the user who created this shopping list.
    /// </summary>
    public User CreatedByUser { get; set; } = null!;

    /// <summary>
    /// Name of the shopping list (e.g., "Woche 1/2026").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Start date of the week this shopping list is for.
    /// </summary>
    public required DateTime ForWeekStartDate { get; set; }

    /// <summary>
    /// End date of the week this shopping list is for.
    /// </summary>
    public required DateTime ForWeekEndDate { get; set; }

    /// <summary>
    /// Whether the shopping list is completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Date when the shopping list was completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Navigation property to shopping list items.
    /// </summary>
    public ICollection<ShoppingListItem> Items { get; set; } = new List<ShoppingListItem>();
}
