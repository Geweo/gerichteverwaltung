using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for ShoppingList entity.
/// </summary>
public class ShoppingListEntityMap : BaseGroupMap<ShoppingList>
{
    public override void Configure(EntityTypeBuilder<ShoppingList> entity)
    {
        base.Configure(entity);

        entity.ToTable("ShoppingLists");

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(e => e.IsCompleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Group relationship
        entity.HasOne(sl => sl.Group)
            .WithMany(g => g.ShoppingLists)
            .HasForeignKey(sl => sl.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // CreatedByUser relationship
        entity.HasOne(sl => sl.CreatedByUser)
            .WithMany(u => u.CreatedShoppingLists)
            .HasForeignKey(sl => sl.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Items relationship
        entity.HasMany(sl => sl.Items)
            .WithOne(sli => sli.ShoppingList)
            .HasForeignKey(sli => sli.ShoppingListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
