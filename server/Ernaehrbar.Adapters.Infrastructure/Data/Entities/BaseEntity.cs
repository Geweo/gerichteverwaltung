namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Base entity with common fields for all entities.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Base entity with group/tenant support for multi-tenant entities.
/// </summary>
public abstract class BaseGroupEntity : BaseEntity
{
    /// <summary>
    /// Foreign key to the group/tenant this entity belongs to.
    /// </summary>
    public int GroupId { get; set; }
    
    /// <summary>
    /// Navigation property to the group.
    /// </summary>
    public Group Group { get; set; } = null!;
}
