using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Parts.Ports;
using Microsoft.EntityFrameworkCore;

namespace Ernaehrbar.Adapters.Infrastructure.Repositories;

/// <summary>
/// Infrastructure adapter implementing IShoppingListRepository port.
/// </summary>
public class ShoppingListRepository : IShoppingListRepository
{
    private readonly ApplicationDbContext _context;

    public ShoppingListRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(ShoppingListDto shoppingList, CancellationToken cancellationToken = default)
    {
        var entity = new ShoppingList
        {
            GroupId = shoppingList.GroupId,
            CreatedByUserId = shoppingList.CreatedByUserId,
            Name = shoppingList.Name,
            ForWeekStartDate = shoppingList.ForWeekStartDate ?? DateTime.UtcNow.Date,
            ForWeekEndDate = shoppingList.ForWeekEndDate ?? DateTime.UtcNow.Date.AddDays(6),
            IsCompleted = shoppingList.IsCompleted,
            CompletedAt = shoppingList.CompletedAt
        };

        if (shoppingList.Items != null)
        {
            foreach (var itemDto in shoppingList.Items)
            {
                entity.Items.Add(new ShoppingListItem
                {
                    IngredientName = itemDto.IngredientName,
                    Quantity = itemDto.Quantity,
                    Unit = itemDto.Unit,
                    RecipeIngredientId = itemDto.RecipeIngredientId,
                    IsChecked = itemDto.IsChecked,
                    Order = itemDto.Order
                });
            }
        }

        _context.ShoppingLists.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<ShoppingListDto?> GetByIdAsync(int shoppingListId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.ShoppingLists
            .Include(sl => sl.Items.OrderBy(i => i.Order))
            .FirstOrDefaultAsync(sl => sl.Id == shoppingListId, cancellationToken);

        if (entity == null)
        {
            return null;
        }

        return MapToDto(entity);
    }

    public async Task UpdateAsync(ShoppingListDto shoppingList, CancellationToken cancellationToken = default)
    {
        if (shoppingList.Id == null)
        {
            throw new ArgumentException("ShoppingList ID is required for update", nameof(shoppingList));
        }

        var entity = await _context.ShoppingLists
            .Include(sl => sl.Items)
            .FirstOrDefaultAsync(sl => sl.Id == shoppingList.Id, cancellationToken);

        if (entity == null)
        {
            throw new InvalidOperationException($"ShoppingList with ID {shoppingList.Id} not found");
        }

        entity.Name = shoppingList.Name;
        entity.ForWeekStartDate = shoppingList.ForWeekStartDate ?? entity.ForWeekStartDate;
        entity.ForWeekEndDate = shoppingList.ForWeekEndDate ?? entity.ForWeekEndDate;
        entity.IsCompleted = shoppingList.IsCompleted;
        entity.CompletedAt = shoppingList.CompletedAt;

        // Update items
        entity.Items.Clear();
        if (shoppingList.Items != null)
        {
            foreach (var itemDto in shoppingList.Items)
            {
                entity.Items.Add(new ShoppingListItem
                {
                    IngredientName = itemDto.IngredientName,
                    Quantity = itemDto.Quantity,
                    Unit = itemDto.Unit,
                    RecipeIngredientId = itemDto.RecipeIngredientId,
                    IsChecked = itemDto.IsChecked,
                    Order = itemDto.Order
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int shoppingListId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.ShoppingLists.FindAsync([shoppingListId], cancellationToken);
        if (entity != null)
        {
            _context.ShoppingLists.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    private static ShoppingListDto MapToDto(ShoppingList entity)
    {
        return new ShoppingListDto(
            Id: entity.Id,
            GroupId: entity.GroupId,
            CreatedByUserId: entity.CreatedByUserId,
            Name: entity.Name,
            ForWeekStartDate: entity.ForWeekStartDate,
            ForWeekEndDate: entity.ForWeekEndDate,
            IsCompleted: entity.IsCompleted,
            CompletedAt: entity.CompletedAt,
            Items: entity.Items.Select(i => new ShoppingListItemDto(
                Id: i.Id,
                IngredientName: i.IngredientName,
                Quantity: i.Quantity,
                Unit: i.Unit,
                RecipeIngredientId: i.RecipeIngredientId,
                IsChecked: i.IsChecked,
                Order: i.Order
            )).ToList()
        );
    }
}
