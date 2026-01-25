using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for Notifications.
/// </summary>
public class NotificationFixture : SeedableFixture<DevelopmentFixtureSet>
{
    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        var users = Parent.UserFixture;
        var drafts = Parent.RecipeDraftFixture;

        var notification1 = new Notification
        {
            UserId = users.MaxMueller.Id,
            Type = NotificationType.UploadComplete,
            Message = "Ein neues Rezept wartet auf deine Überprüfung",
            IsRead = false,
            ActionLink = drafts.PendingDraft.Id.ToString()
        };
        await Context.Notifications.AddAsync(notification1, cancellationToken);

        var notification2 = new Notification
        {
            UserId = users.AnnaMueller.Id,
            Type = NotificationType.MealPlanReady,
            Message = "Dein Wochenplan wurde erfolgreich erstellt",
            IsRead = true,
            ReadAt = DateTime.UtcNow.AddHours(-2)
        };
        await Context.Notifications.AddAsync(notification2, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken);
    }
}
