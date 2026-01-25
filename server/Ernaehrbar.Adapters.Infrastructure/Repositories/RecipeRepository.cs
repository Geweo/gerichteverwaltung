using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Ports;
using Microsoft.EntityFrameworkCore;
using DomainRecipeSource = Ernaehrbar.Parts.Domain.RecipeSource;
using InfrastructureRecipeSource = Ernaehrbar.Adapters.Infrastructure.Data.Entities.RecipeSource;

namespace Ernaehrbar.Adapters.Infrastructure.Repositories;

/// <summary>
/// Infrastructure adapter implementing IRecipeRepository port.
/// </summary>
public class RecipeRepository : IRecipeRepository
{
    private readonly ApplicationDbContext _context;

    public RecipeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(RecipeDto recipe, CancellationToken cancellationToken = default)
    {
        var entity = new Recipe
        {
            GroupId = recipe.GroupId,
            Name = recipe.Name,
            Source = MapRecipeSourceToInfrastructure(recipe.Source),
            Description = recipe.Description,
            Instructions = recipe.Instructions,
            MealCategory = recipe.MealCategory,
            Servings = recipe.Servings,
            PreparationTimeMinutes = recipe.PreparationTimeMinutes,
            CookingTimeMinutes = recipe.CookingTimeMinutes,
            RepeatCycleWeeks = recipe.RepeatCycleWeeks
        };

        if (recipe.Ingredients != null)
        {
            foreach (var ingredientDto in recipe.Ingredients)
            {
                entity.Ingredients.Add(new RecipeIngredient
                {
                    Name = ingredientDto.Name,
                    Quantity = ingredientDto.Quantity,
                    Unit = ingredientDto.Unit,
                    Notes = ingredientDto.Notes,
                    Order = ingredientDto.Order
                });
            }
        }

        _context.Recipes.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task UpdateAsync(RecipeDto recipe, CancellationToken cancellationToken = default)
    {
        if (recipe.Id == null)
        {
            throw new ArgumentException("Recipe ID is required for update", nameof(recipe));
        }

        var entity = await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == recipe.Id, cancellationToken);

        if (entity == null)
        {
            throw new InvalidOperationException($"Recipe with ID {recipe.Id} not found");
        }

        entity.Name = recipe.Name;
        entity.Source = MapRecipeSourceToInfrastructure(recipe.Source);
        entity.Description = recipe.Description;
        entity.Instructions = recipe.Instructions;
        entity.MealCategory = recipe.MealCategory;
        entity.Servings = recipe.Servings;
        entity.PreparationTimeMinutes = recipe.PreparationTimeMinutes;
        entity.CookingTimeMinutes = recipe.CookingTimeMinutes;
        entity.RepeatCycleWeeks = recipe.RepeatCycleWeeks;

        // Update ingredients
        entity.Ingredients.Clear();
        if (recipe.Ingredients != null)
        {
            foreach (var ingredientDto in recipe.Ingredients)
            {
                entity.Ingredients.Add(new RecipeIngredient
                {
                    Name = ingredientDto.Name,
                    Quantity = ingredientDto.Quantity,
                    Unit = ingredientDto.Unit,
                    Notes = ingredientDto.Notes,
                    Order = ingredientDto.Order
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int recipeId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Recipes.FindAsync([recipeId], cancellationToken);
        if (entity != null)
        {
            _context.Recipes.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    private static InfrastructureRecipeSource MapRecipeSourceToInfrastructure(DomainRecipeSource source)
    {
        return source switch
        {
            DomainRecipeSource.Generated => InfrastructureRecipeSource.Generated,
            DomainRecipeSource.Upload => InfrastructureRecipeSource.Upload,
            DomainRecipeSource.Manual => InfrastructureRecipeSource.Manual,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }
}
