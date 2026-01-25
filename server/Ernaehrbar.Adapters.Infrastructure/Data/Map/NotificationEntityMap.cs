using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for Notification entity.
/// </summary>
public class NotificationEntityMap : BaseMap<Notification>
{
    public override void Configure(EntityTypeBuilder<Notification> entity)
    {
        base.Configure(entity);

        entity.ToTable("Notifications");

        entity.Property(e => e.Message)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(e => e.ActionLink)
            .HasMaxLength(500);

        entity.Property(e => e.IsRead)
            .IsRequired()
            .HasDefaultValue(false);

        // User relationship
        entity.HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
