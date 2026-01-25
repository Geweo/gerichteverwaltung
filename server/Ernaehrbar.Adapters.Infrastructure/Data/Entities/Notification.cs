namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Notification for a user.
/// </summary>
public class Notification : BaseEntity
{
    /// <summary>
    /// Foreign key to the user who receives this notification.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to the user.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Type of notification.
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Notification message.
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// Whether the notification has been read.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Date when the notification was read.
    /// </summary>
    public DateTime? ReadAt { get; set; }

    /// <summary>
    /// Optional link to a resource (e.g., RecipeDraft ID, MealPlan ID).
    /// </summary>
    public string? ActionLink { get; set; }
}
