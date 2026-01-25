using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for GroupMember (join table) entity.
/// </summary>
public class GroupMemberEntityMap : BaseMap<GroupMember>
{
    public override void Configure(EntityTypeBuilder<GroupMember> entity)
    {
        base.Configure(entity);

        entity.ToTable("GroupMembers");

        // Composite unique constraint (User + Group)
        entity.HasIndex(gm => new { gm.UserId, gm.GroupId })
            .IsUnique();

        // User relationship
        entity.HasOne(gm => gm.User)
            .WithMany(u => u.GroupMemberships)
            .HasForeignKey(gm => gm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Group relationship
        entity.HasOne(gm => gm.Group)
            .WithMany(g => g.Members)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
