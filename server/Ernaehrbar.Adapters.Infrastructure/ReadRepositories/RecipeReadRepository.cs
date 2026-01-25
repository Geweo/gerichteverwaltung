using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Ports;
using Ernaehrbar.Parts.Queries.Common;
using Ernaehrbar.Parts.ReadModels;
using Microsoft.EntityFrameworkCore;
using RecipeSource = Ernaehrbar.Parts.Domain.RecipeSource;

namespace Ernaehrbar.Adapters.Infrastructure.ReadRepositories;

/// <summary>
/// Infrastructure-Adapter für IRecipeReadRepository: Read-optimierte Rezept-Abfragen mit EF Core.
/// </summary>
public class RecipeReadRepository : IRecipeReadRepository
{
    private readonly ApplicationDbContext _context;

    public RecipeReadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<RecipeReadModel?> GetByIdAsync(int recipeId, CancellationToken cancellationToken = default)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients.OrderBy(i => i.Order))
            .Include(r => r.RecipeTags)
                .ThenInclude(rt => rt.Tag)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == recipeId, cancellationToken);

        if (recipe is null)
        {
            return null;
        }

        return MapToReadModel(recipe);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<RecipeReadModel>> GetRecipesAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = _context.Recipes
            .Include(r => r.Ingredients.OrderBy(i => i.Order))
            .Include(r => r.RecipeTags)
                .ThenInclude(rt => rt.Tag)
            .AsNoTracking()
            .Where(r => r.GroupId == groupId);

        // Filter nach Tags
        if (tagIds is not null && tagIds.Count > 0)
        {
            query = query.Where(r => r.RecipeTags.Any(rt => tagIds.Contains(rt.TagId)));
        }

        // Suche nach Name oder Beschreibung
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(r =>
                r.Name.ToLower().Contains(searchLower) ||
                (r.Description != null && r.Description.ToLower().Contains(searchLower)));
        }

        // Filter nach MealCategory
        if (mealCategory.HasValue)
        {
            query = query.Where(r => r.MealCategory == mealCategory.Value);
        }

        // Filter nach Source
        if (source.HasValue)
        {
            query = query.Where(r => r.Source == source.Value);
        }

        // Filter nach Favorites
        // TODO: Implementieren, wenn UserId aus Context verfügbar ist
        // Favorites werden über RecipeRating.IsFavorite gespeichert
        // if (favorites.HasValue && favorites.Value)
        // {
        //     var userId = GetUserIdFromContext(); // TODO: Implementieren
        //     query = query.Where(r => r.Ratings.Any(rt => rt.UserId == userId && rt.IsFavorite));
        // }

        // Apply sorting
        query = sortBy switch
        {
            RecipeListSorting.Name => sortDirection == SortDirectionEnum.Desc
                ? query.OrderByDescending(r => r.Name)
                : query.OrderBy(r => r.Name),
            RecipeListSorting.CreatedAt => sortDirection == SortDirectionEnum.Desc
                ? query.OrderByDescending(r => r.CreatedAt)
                : query.OrderBy(r => r.CreatedAt),
            RecipeListSorting.UpdatedAt => sortDirection == SortDirectionEnum.Desc
                ? query.OrderByDescending(r => r.UpdatedAt)
                : query.OrderBy(r => r.UpdatedAt),
            RecipeListSorting.MealCategory => sortDirection == SortDirectionEnum.Desc
                ? query.OrderByDescending(r => r.MealCategory)
                : query.OrderBy(r => r.MealCategory),
            RecipeListSorting.Source => sortDirection == SortDirectionEnum.Desc
                ? query.OrderByDescending(r => r.Source)
                : query.OrderBy(r => r.Source),
            RecipeListSorting.AverageRating => sortDirection == SortDirectionEnum.Desc
                ? query.OrderByDescending(r => r.Ratings.Any() ? r.Ratings.Average(rt => rt.Rating) : 0)
                : query.OrderBy(r => r.Ratings.Any() ? r.Ratings.Average(rt => rt.Rating) : 0),
            _ => query.OrderBy(r => r.Name),
        };

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination (1-based to 0-based)
        var skip = (page - 1) * pageSize;
        var recipes = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = recipes.Select(MapToReadModel).ToList();

        return new PaginatedResult<RecipeReadModel>(page, pageSize, totalCount, items);
    }

    private static RecipeReadModel MapToReadModel(Recipe recipe)
    {
        return new RecipeReadModel(
            Id: recipe.Id,
            GroupId: recipe.GroupId,
            Name: recipe.Name,
            Description: recipe.Description,
            Instructions: recipe.Instructions,
            ImageUrl: recipe.ImageUrl,
            PdfUrl: recipe.PdfUrl,
            Servings: recipe.Servings,
            PreparationTimeMinutes: recipe.PreparationTimeMinutes,
            CookingTimeMinutes: recipe.CookingTimeMinutes,
            CreatedAt: recipe.CreatedAt,
            UpdatedAt: recipe.UpdatedAt,
            Ingredients: recipe.Ingredients.Select(i => new RecipeIngredientReadModel(
                Id: i.Id,
                Name: i.Name,
                Quantity: i.Quantity,
                Unit: i.Unit,
                Notes: i.Notes,
                Order: i.Order)).ToList(),
            Tags: recipe.RecipeTags.Select(rt => rt.Tag.Name).ToList());
    }
}
