using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Base map configuration for entities with common fields (Id, CreatedAt, UpdatedAt).
/// </summary>
public abstract class BaseMap<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        entity.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}

/// <summary>
/// Base map configuration for entities with GroupId (multi-tenant entities).
/// </summary>
public abstract class BaseGroupMap<T> : BaseMap<T>
    where T : BaseGroupEntity
{
    public override void Configure(EntityTypeBuilder<T> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.GroupId)
            .IsRequired();

        entity.HasIndex(e => e.GroupId);
    }
}
