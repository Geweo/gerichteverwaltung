namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents the many-to-many relationship between recipes and tags.
/// </summary>
public class RecipeTag : BaseEntity
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
    /// Foreign key to the tag.
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// Navigation property to the tag.
    /// </summary>
    public Tag Tag { get; set; } = null!;
}
