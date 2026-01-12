namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents an invitation to join a group.
/// </summary>
public class GroupInvite : BaseEntity
{
    /// <summary>
    /// Foreign key to the group.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Navigation property to the group.
    /// </summary>
    public Group Group { get; set; } = null!;

    /// <summary>
    /// Unique token for the invitation.
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// Email address of the invited user (optional, can be null for open invites).
    /// </summary>
    public string? InvitedEmail { get; set; }

    /// <summary>
    /// User who created the invitation.
    /// </summary>
    public int CreatedByUserId { get; set; }

    /// <summary>
    /// Navigation property to the user who created the invitation.
    /// </summary>
    public User CreatedByUser { get; set; } = null!;

    /// <summary>
    /// Expiration date of the invitation.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Whether the invitation has been used.
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// Date when the invitation was used (if used).
    /// </summary>
    public DateTime? UsedAt { get; set; }
}
