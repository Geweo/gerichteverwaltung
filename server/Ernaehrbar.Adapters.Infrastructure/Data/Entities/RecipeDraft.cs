using Ernaehrbar.Parts.Ports;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents a recipe draft during review process (before approval).
/// </summary>
public class RecipeDraft : BaseGroupEntity
{
    /// <summary>
    /// Status of the draft (Pending, Approved, Rejected).
    /// </summary>
    public DraftStatus Status { get; set; } = DraftStatus.Pending;

    /// <summary>
    /// Source of the draft (Generated, Upload).
    /// </summary>
    public RecipeSource Source { get; set; }

    /// <summary>
    /// Original data from upload/KI generation (JSON).
    /// </summary>
    public string? OriginalData { get; set; }

    /// <summary>
    /// Foreign key to the user who created this draft.
    /// </summary>
    public int CreatedByUserId { get; set; }

    /// <summary>
    /// Navigation property to the user who created this draft.
    /// </summary>
    public User CreatedByUser { get; set; } = null!;

    /// <summary>
    /// Foreign key to the user who reviewed this draft (nullable).
    /// </summary>
    public int? ReviewedByUserId { get; set; }

    /// <summary>
    /// Navigation property to the user who reviewed this draft.
    /// </summary>
    public User? ReviewedByUser { get; set; }

    /// <summary>
    /// Date when the draft was reviewed.
    /// </summary>
    public DateTime? ReviewedAt { get; set; }

    /// <summary>
    /// Name of the recipe (editierbar).
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
    /// Meal category (nullable).
    /// </summary>
    public MealCategory? MealCategory { get; set; }

    /// <summary>
    /// Navigation property to draft ingredients.
    /// </summary>
    public ICollection<RecipeDraftIngredient> Ingredients { get; set; } = new List<RecipeDraftIngredient>();

    /// <summary>
    /// Navigation property to files associated with this draft.
    /// </summary>
    public ICollection<Entities.File> Files { get; set; } = new List<Entities.File>();
}
