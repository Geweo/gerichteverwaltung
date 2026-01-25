using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for User entity.
/// Note: User doesn't inherit from BaseGroupEntity, so we use BaseMap.
/// </summary>
public class UserEntityMap : BaseMap<User>
{
    public override void Configure(EntityTypeBuilder<User> entity)
    {
        base.Configure(entity);

        entity.ToTable("Users");

        entity.Property(e => e.SupabaseUserId)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(e => e.DisplayName)
            .HasMaxLength(200);

        // Unique constraint on SupabaseUserId
        entity.HasIndex(u => u.SupabaseUserId)
            .IsUnique();

        entity.HasIndex(u => u.Email);
    }
}
