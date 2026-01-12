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
}
