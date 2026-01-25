using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for GroupInvite entity.
/// </summary>
public class GroupInviteEntityMap : BaseMap<GroupInvite>
{
    public override void Configure(EntityTypeBuilder<GroupInvite> entity)
    {
        base.Configure(entity);

        entity.ToTable("GroupInvites");

        entity.Property(e => e.Token)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(e => e.InvitedEmail)
            .HasMaxLength(255);

        // Unique constraint on Token
        entity.HasIndex(gi => gi.Token)
            .IsUnique();

        // Group relationship
        entity.HasOne(gi => gi.Group)
            .WithMany(g => g.Invites)
            .HasForeignKey(gi => gi.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // CreatedByUser relationship
        entity.HasOne(gi => gi.CreatedByUser)
            .WithMany()
            .HasForeignKey(gi => gi.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
