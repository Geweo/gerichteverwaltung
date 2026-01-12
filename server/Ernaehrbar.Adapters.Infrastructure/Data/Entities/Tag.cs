namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents a tag that can be assigned to recipes.
/// Tags are categorized by type (Preparation, Diet, Ingredient).
/// </summary>
public class Tag : BaseGroupEntity
{
    /// <summary>
    /// Name of the tag (e.g., "vegetarisch", "schnell", "Kidneybohnen").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Category/type of the tag.
    /// </summary>
    public TagCategory Category { get; set; }

    /// <summary>
    /// Navigation property to recipe tags.
    /// </summary>
    public ICollection<RecipeTag> RecipeTags { get; set; } = new List<RecipeTag>();
}

/// <summary>
/// Category of a tag.
/// </summary>
public enum TagCategory
{
    /// <summary>
    /// Preparation method (e.g., "schnell", "aufwendig", "einfach").
    /// </summary>
    Preparation = 1,

    /// <summary>
    /// Diet type (e.g., "vegetarisch", "vegan", "low-carb", "glutenfrei").
    /// </summary>
    Diet = 2,

    /// <summary>
    /// Ingredient-based (e.g., "Kidneybohnen", "Fisch", "Wurst").
    /// </summary>
    Ingredient = 3
}
