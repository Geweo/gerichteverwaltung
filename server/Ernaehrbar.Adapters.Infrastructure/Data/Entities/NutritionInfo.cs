namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents nutrition information for a recipe.
/// </summary>
public class NutritionInfo : BaseEntity
{
    /// <summary>
    /// Foreign key to the recipe.
    /// </summary>
    public int RecipeId { get; set; }

    /// <summary>
    /// Navigation property to the recipe.
    /// </summary>
    public Recipe Recipe { get; set; } = null!;

    /// <summary>
    /// Calories per serving.
    /// </summary>
    public decimal? Calories { get; set; }

    /// <summary>
    /// Protein in grams per serving.
    /// </summary>
    public decimal? Protein { get; set; }

    /// <summary>
    /// Carbohydrates in grams per serving.
    /// </summary>
    public decimal? Carbohydrates { get; set; }

    /// <summary>
    /// Fat in grams per serving.
    /// </summary>
    public decimal? Fat { get; set; }

    /// <summary>
    /// Fiber in grams per serving.
    /// </summary>
    public decimal? Fiber { get; set; }

    /// <summary>
    /// Sugar in grams per serving.
    /// </summary>
    public decimal? Sugar { get; set; }

    /// <summary>
    /// Sodium in milligrams per serving.
    /// </summary>
    public decimal? Sodium { get; set; }
}
