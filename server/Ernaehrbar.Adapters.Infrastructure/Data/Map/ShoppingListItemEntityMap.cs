using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for ShoppingListItem entity.
/// </summary>
public class ShoppingListItemEntityMap : BaseMap<ShoppingListItem>
{
    public override void Configure(EntityTypeBuilder<ShoppingListItem> entity)
    {
        base.Configure(entity);

        entity.ToTable("ShoppingListItems");

        entity.Property(e => e.IngredientName)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(e => e.Quantity)
            .HasMaxLength(50);

        entity.Property(e => e.Unit)
            .HasMaxLength(50);

        entity.Property(e => e.IsChecked)
            .IsRequired()
            .HasDefaultValue(false);

        // ShoppingList relationship
        entity.HasOne(sli => sli.ShoppingList)
            .WithMany(sl => sl.Items)
            .HasForeignKey(sli => sli.ShoppingListId)
            .OnDelete(DeleteBehavior.Cascade);

        // RecipeIngredient relationship (nullable)
        entity.HasOne(sli => sli.RecipeIngredient)
            .WithMany()
            .HasForeignKey(sli => sli.RecipeIngredientId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
