using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Parts.Ports;
using Ernaehrbar.Parts.ReadModels;
using Microsoft.EntityFrameworkCore;

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
            .FirstOrDefaultAsync(r => r.Id == recipeId, cancellationToken);

        if (recipe is null)
        {
            return null;
        }

        return MapToReadModel(recipe);
    }

    /// <inheritdoc />
    public async Task<List<RecipeReadModel>> GetRecipesAsync(
        int groupId,
        List<int>? tagIds = null,
        string? searchTerm = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Recipes
            .Include(r => r.Ingredients.OrderBy(i => i.Order))
            .Include(r => r.RecipeTags)
                .ThenInclude(rt => rt.Tag)
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

        // Pagination
        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        var recipes = await query.ToListAsync(cancellationToken);

        return recipes.Select(MapToReadModel).ToList();
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
