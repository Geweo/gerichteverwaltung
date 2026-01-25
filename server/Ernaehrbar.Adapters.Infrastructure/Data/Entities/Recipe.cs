using Ernaehrbar.Parts.Ports;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents a recipe that belongs to a group.
/// </summary>
public class Recipe : BaseGroupEntity
{
    /// <summary>
    /// Name of the recipe.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Description of the recipe.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Cooking instructions/process.
    /// </summary>
    public string? Instructions { get; set; }

    /// <summary>
    /// URL to the recipe image (stored in Supabase Storage).
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// URL to the original recipe PDF (if uploaded).
    /// </summary>
    public string? PdfUrl { get; set; }

    /// <summary>
    /// Number of servings.
    /// </summary>
    public int? Servings { get; set; }

    /// <summary>
    /// Preparation time in minutes.
    /// </summary>
    public int? PreparationTimeMinutes { get; set; }

    /// <summary>
    /// Cooking time in minutes.
    /// </summary>
    public int? CookingTimeMinutes { get; set; }

    /// <summary>
    /// Source/Origin of the recipe (Generated, Upload, Manual).
    /// </summary>
    public RecipeSource Source { get; set; } = RecipeSource.Manual;

    /// <summary>
    /// Meal category this recipe is typically used for (nullable, can be flexible).
    /// </summary>
    public MealCategory? MealCategory { get; set; }

    /// <summary>
    /// Repeat cycle in weeks (e.g., 2 = repeat every 2 weeks).
    /// Null means no automatic repetition.
    /// </summary>
    public int? RepeatCycleWeeks { get; set; }

    /// <summary>
    /// Navigation property to ingredients.
    /// </summary>
    public ICollection<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();

    /// <summary>
    /// Navigation property to recipe tags.
    /// </summary>
    public ICollection<RecipeTag> RecipeTags { get; set; } = new List<RecipeTag>();

    /// <summary>
    /// Navigation property to ratings.
    /// </summary>
    public ICollection<RecipeRating> Ratings { get; set; } = new List<RecipeRating>();

    /// <summary>
    /// Navigation property to nutrition information.
    /// </summary>
    public NutritionInfo? NutritionInfo { get; set; }

    /// <summary>
    /// Navigation property to meal plan entries.
    /// </summary>
    public ICollection<MealPlanEntry> MealPlanEntries { get; set; } = new List<MealPlanEntry>();

    /// <summary>
    /// Navigation property to files associated with this recipe.
    /// </summary>
    public ICollection<Entities.File> Files { get; set; } = new List<Entities.File>();
}
