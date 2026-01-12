namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents a user's rating and favorite status for a recipe.
/// </summary>
public class RecipeRating : BaseEntity
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
    /// Foreign key to the user who rated the recipe.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to the user.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Rating from 1 to 5 (null if not rated).
    /// </summary>
    public int? Rating { get; set; }

    /// <summary>
    /// Whether the recipe is marked as favorite by this user.
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <summary>
    /// Optional comment/review.
    /// </summary>
    public string? Comment { get; set; }
}
