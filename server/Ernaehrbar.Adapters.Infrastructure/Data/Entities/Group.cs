namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents a group/tenant that can contain multiple users.
/// Groups share recipes, meal plans, and shopping lists.
/// </summary>
public class Group : BaseEntity
{
    /// <summary>
    /// Name of the group (e.g., "Familie Müller", "WG Berlin").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Optional description of the group.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Navigation property to group members.
    /// </summary>
    public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();

    /// <summary>
    /// Navigation property to recipes belonging to this group.
    /// </summary>
    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    /// <summary>
    /// Navigation property to meal plans belonging to this group.
    /// </summary>
    public ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();

    /// <summary>
    /// Navigation property to group invites.
    /// </summary>
    public ICollection<GroupInvite> Invites { get; set; } = new List<GroupInvite>();

    /// <summary>
    /// Navigation property to recipe drafts belonging to this group.
    /// </summary>
    public ICollection<RecipeDraft> RecipeDrafts { get; set; } = new List<RecipeDraft>();

    /// <summary>
    /// Navigation property to upload tasks belonging to this group.
    /// </summary>
    public ICollection<UploadTask> UploadTasks { get; set; } = new List<UploadTask>();

    /// <summary>
    /// Navigation property to files belonging to this group.
    /// </summary>
    public ICollection<File> Files { get; set; } = new List<File>();

    /// <summary>
    /// Navigation property to shopping lists belonging to this group.
    /// </summary>
    public ICollection<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
}
