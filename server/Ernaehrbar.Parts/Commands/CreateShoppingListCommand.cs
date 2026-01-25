using MediatR;

namespace Ernaehrbar.Parts.Commands;

/// <summary>
/// Command zum Erstellen einer ShoppingList.
/// </summary>
public record CreateShoppingListCommand(
    int GroupId,
    int CreatedByUserId,
    string Name,
    DateTime? ForWeekStartDate = null,
    DateTime? ForWeekEndDate = null,
    List<CreateShoppingListItemDto>? Items = null
) : IRequest<CreateShoppingListResult>;

/// <summary>
/// DTO für ShoppingList-Items beim Erstellen.
/// </summary>
public record CreateShoppingListItemDto(
    string IngredientName,
    decimal? Quantity = null,
    string? Unit = null,
    int? RecipeIngredientId = null
);

/// <summary>
/// Result für CreateShoppingListCommand.
/// </summary>
public record CreateShoppingListResult(
    int Id
);
