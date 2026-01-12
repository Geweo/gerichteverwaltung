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
}
