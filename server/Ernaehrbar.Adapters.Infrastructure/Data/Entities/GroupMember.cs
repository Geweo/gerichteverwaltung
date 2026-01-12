namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents the relationship between a user and a group.
/// Defines the role of a user within a group.
/// </summary>
public class GroupMember : BaseEntity
{
    /// <summary>
    /// Foreign key to the user.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to the user.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Foreign key to the group.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Navigation property to the group.
    /// </summary>
    public Group Group { get; set; } = null!;

    /// <summary>
    /// Role of the user in the group.
    /// </summary>
    public GroupRole Role { get; set; } = GroupRole.Member;

    /// <summary>
    /// Date when the user joined the group.
    /// </summary>
    public DateTime JoinedAt { get; set; }
}

/// <summary>
/// Role of a user within a group.
/// </summary>
public enum GroupRole
{
    /// <summary>
    /// Regular member with standard permissions.
    /// </summary>
    Member = 1,

    /// <summary>
    /// Admin with full permissions (can manage members, delete group, etc.).
    /// </summary>
    Admin = 2
}
