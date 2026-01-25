namespace Ernaehrbar.Parts.Ports;

/// <summary>
/// Port for shopping list repository operations (write).
/// </summary>
public interface IShoppingListRepository
{
    /// <summary>
    /// Adds a new shopping list.
    /// </summary>
    Task<int> AddAsync(ShoppingListDto shoppingList, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a shopping list by ID.
    /// </summary>
    Task<ShoppingListDto?> GetByIdAsync(int shoppingListId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a shopping list.
    /// </summary>
    Task UpdateAsync(ShoppingListDto shoppingList, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a shopping list.
    /// </summary>
    Task DeleteAsync(int shoppingListId, CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO for shopping list operations (write).
/// </summary>
public record ShoppingListDto(
    int? Id,
    int GroupId,
    int CreatedByUserId,
    string Name,
    DateTime? ForWeekStartDate = null,
    DateTime? ForWeekEndDate = null,
    bool IsCompleted = false,
    DateTime? CompletedAt = null,
    List<ShoppingListItemDto>? Items = null
);

/// <summary>
/// DTO for shopping list item.
/// </summary>
public record ShoppingListItemDto(
    int? Id,
    string IngredientName,
    decimal? Quantity = null,
    string? Unit = null,
    int? RecipeIngredientId = null,
    bool IsChecked = false,
    int Order = 0
);
