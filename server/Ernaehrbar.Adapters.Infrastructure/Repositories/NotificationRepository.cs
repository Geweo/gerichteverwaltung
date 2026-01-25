using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Ports;
using Microsoft.EntityFrameworkCore;
using DomainNotificationType = Ernaehrbar.Parts.Domain.NotificationType;
using InfrastructureNotificationType = Ernaehrbar.Adapters.Infrastructure.Data.Entities.NotificationType;

namespace Ernaehrbar.Adapters.Infrastructure.Repositories;

/// <summary>
/// Infrastructure adapter implementing INotificationRepository port.
/// </summary>
public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(NotificationDto notification, CancellationToken cancellationToken = default)
    {
        var entity = new Notification
        {
            UserId = notification.UserId,
            Type = MapNotificationTypeToInfrastructure(notification.Type),
            Message = notification.Message,
            IsRead = notification.IsRead,
            ReadAt = notification.ReadAt,
            ActionLink = notification.ActionLink
        };

        _context.Notifications.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<NotificationDto?> GetByIdAsync(int notificationId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId, cancellationToken);

        if (entity == null)
        {
            return null;
        }

        return MapToDto(entity);
    }

    public async Task UpdateAsync(NotificationDto notification, CancellationToken cancellationToken = default)
    {
        if (notification.Id == null)
        {
            throw new ArgumentException("Notification ID is required for update", nameof(notification));
        }

        var entity = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notification.Id, cancellationToken);

        if (entity == null)
        {
            throw new InvalidOperationException($"Notification with ID {notification.Id} not found");
        }

        entity.Type = MapNotificationTypeToInfrastructure(notification.Type);
        entity.Message = notification.Message;
        entity.IsRead = notification.IsRead;
        entity.ReadAt = notification.ReadAt;
        entity.ActionLink = notification.ActionLink;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int notificationId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Notifications.FindAsync([notificationId], cancellationToken);
        if (entity != null)
        {
            _context.Notifications.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    private static NotificationDto MapToDto(Notification entity)
    {
        return new NotificationDto(
            Id: entity.Id,
            UserId: entity.UserId,
            Type: MapNotificationTypeToDomain(entity.Type),
            Message: entity.Message,
            IsRead: entity.IsRead,
            ReadAt: entity.ReadAt,
            ActionLink: entity.ActionLink
        );
    }

    private static DomainNotificationType MapNotificationTypeToDomain(InfrastructureNotificationType type)
    {
        return type switch
        {
            InfrastructureNotificationType.UploadComplete => DomainNotificationType.UploadComplete,
            InfrastructureNotificationType.GenerationComplete => DomainNotificationType.GenerationComplete,
            InfrastructureNotificationType.MealPlanReady => DomainNotificationType.MealPlanReady,
            InfrastructureNotificationType.SimilarRecipeWarning => DomainNotificationType.SimilarRecipeWarning,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private static InfrastructureNotificationType MapNotificationTypeToInfrastructure(DomainNotificationType type)
    {
        return type switch
        {
            DomainNotificationType.UploadComplete => InfrastructureNotificationType.UploadComplete,
            DomainNotificationType.GenerationComplete => InfrastructureNotificationType.GenerationComplete,
            DomainNotificationType.MealPlanReady => InfrastructureNotificationType.MealPlanReady,
            DomainNotificationType.SimilarRecipeWarning => InfrastructureNotificationType.SimilarRecipeWarning,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
