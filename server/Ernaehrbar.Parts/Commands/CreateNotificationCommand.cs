using Ernaehrbar.Parts.Domain;
using MediatR;

namespace Ernaehrbar.Parts.Commands;

/// <summary>
/// Command zum Erstellen einer Notification.
/// </summary>
public record CreateNotificationCommand(
    int UserId,
    NotificationType Type,
    string Message,
    string? ActionLink = null
) : IRequest<CreateNotificationResult>;

/// <summary>
/// Result für CreateNotificationCommand.
/// </summary>
public record CreateNotificationResult(
    int Id
);
