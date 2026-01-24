using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Parts.Ports;
using Ernaehrbar.Parts.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Ernaehrbar.Adapters.Infrastructure.ReadRepositories;

/// <summary>
/// Infrastructure-Adapter für IMealPlanReadRepository: Read-optimierte Wochenplan-Abfragen mit EF Core.
/// </summary>
public class MealPlanReadRepository : IMealPlanReadRepository
{
    private readonly ApplicationDbContext _context;

    public MealPlanReadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<MealPlanReadModel?> GetByIdAsync(int mealPlanId, CancellationToken cancellationToken = default)
    {
        var mealPlan = await _context.MealPlans
            .Include(mp => mp.Entries.OrderBy(e => e.Date).ThenBy(e => e.MealCategory))
                .ThenInclude(e => e.Recipe)
                    .ThenInclude(r => r!.Ingredients.OrderBy(i => i.Order))
            .Include(mp => mp.Entries)
                .ThenInclude(e => e.Recipe)
                    .ThenInclude(r => r!.RecipeTags)
                        .ThenInclude(rt => rt.Tag)
            .FirstOrDefaultAsync(mp => mp.Id == mealPlanId, cancellationToken);

        if (mealPlan is null)
        {
            return null;
        }

        return MapToReadModel(mealPlan);
    }

    /// <inheritdoc />
    public async Task<List<MealPlanReadModel>> GetMealPlansAsync(
        int groupId,
        DateTime? startDateFrom = null,
        DateTime? startDateTo = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.MealPlans
            .Include(mp => mp.Entries.OrderBy(e => e.Date).ThenBy(e => e.MealCategory))
                .ThenInclude(e => e.Recipe)
                    .ThenInclude(r => r!.Ingredients.OrderBy(i => i.Order))
            .Include(mp => mp.Entries)
                .ThenInclude(e => e.Recipe)
                    .ThenInclude(r => r!.RecipeTags)
                        .ThenInclude(rt => rt.Tag)
            .Where(mp => mp.GroupId == groupId);

        // Filter nach StartDate
        if (startDateFrom.HasValue)
        {
            query = query.Where(mp => mp.StartDate >= startDateFrom.Value);
        }

        if (startDateTo.HasValue)
        {
            query = query.Where(mp => mp.StartDate <= startDateTo.Value);
        }

        // Sortierung nach StartDate (neueste zuerst)
        query = query.OrderByDescending(mp => mp.StartDate);

        // Pagination
        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        var mealPlans = await query.ToListAsync(cancellationToken);

        return mealPlans.Select(MapToReadModel).ToList();
    }

    private static MealPlanReadModel MapToReadModel(MealPlan mealPlan)
    {
        return new MealPlanReadModel(
            Id: mealPlan.Id,
            GroupId: mealPlan.GroupId,
            StartDate: mealPlan.StartDate,
            EndDate: mealPlan.EndDate,
            Name: mealPlan.Name,
            GenerationPrompt: mealPlan.GenerationPrompt,
            CreatedAt: mealPlan.CreatedAt,
            UpdatedAt: mealPlan.UpdatedAt,
            Entries: mealPlan.Entries.Select(e => new MealPlanEntryReadModel(
                Id: e.Id,
                MealPlanId: e.MealPlanId,
                RecipeId: e.RecipeId ?? 0,
                Date: e.Date,
                MealCategory: e.MealCategory,
                DayNumber: e.DayNumber,
                Recipe: e.Recipe != null ? MapRecipeToReadModel(e.Recipe) : null)).ToList());
    }

    private static RecipeReadModel MapRecipeToReadModel(Recipe recipe)
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
