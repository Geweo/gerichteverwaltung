namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents a user in the system.
/// Maps to Supabase user via SupabaseUserId (sub from JWT token).
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Supabase user ID (sub from JWT token).
    /// </summary>
    public required string SupabaseUserId { get; set; }

    /// <summary>
    /// Email address of the user (from Supabase).
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Display name of the user.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Navigation property to group memberships.
    /// </summary>
    public ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();

    /// <summary>
    /// Navigation property to recipe drafts created by this user.
    /// </summary>
    public ICollection<RecipeDraft> CreatedRecipeDrafts { get; set; } = new List<RecipeDraft>();

    /// <summary>
    /// Navigation property to recipe drafts reviewed by this user.
    /// </summary>
    public ICollection<RecipeDraft> ReviewedRecipeDrafts { get; set; } = new List<RecipeDraft>();

    /// <summary>
    /// Navigation property to upload tasks created by this user.
    /// </summary>
    public ICollection<UploadTask> UploadTasks { get; set; } = new List<UploadTask>();

    /// <summary>
    /// Navigation property to files uploaded by this user.
    /// </summary>
    public ICollection<File> UploadedFiles { get; set; } = new List<File>();

    /// <summary>
    /// Navigation property to shopping lists created by this user.
    /// </summary>
    public ICollection<ShoppingList> CreatedShoppingLists { get; set; } = new List<ShoppingList>();

    /// <summary>
    /// Navigation property to notifications for this user.
    /// </summary>
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
