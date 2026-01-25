using Ernaehrbar.Parts.Domain;

namespace Ernaehrbar.Parts.Ports;

/// <summary>
/// Port for notification repository operations (write).
/// </summary>
public interface INotificationRepository
{
    /// <summary>
    /// Adds a new notification.
    /// </summary>
    Task<int> AddAsync(NotificationDto notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a notification by ID.
    /// </summary>
    Task<NotificationDto?> GetByIdAsync(int notificationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a notification.
    /// </summary>
    Task UpdateAsync(NotificationDto notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a notification.
    /// </summary>
    Task DeleteAsync(int notificationId, CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO for notification operations (write).
/// </summary>
public record NotificationDto(
    int? Id,
    int UserId,
    NotificationType Type,
    string Message,
    bool IsRead = false,
    DateTime? ReadAt = null,
    string? ActionLink = null
);
