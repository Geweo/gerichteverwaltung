using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for Group entity.
/// </summary>
public class GroupEntityMap : BaseMap<Group>
{
    public override void Configure(EntityTypeBuilder<Group> entity)
    {
        base.Configure(entity);

        entity.ToTable("Groups");

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(e => e.Description)
            .HasMaxLength(1000);

        // Navigation properties (configured in other maps)
        // - Members (GroupMember)
        // - Recipes
        // - MealPlans
        // - RecipeDrafts
        // - Tags
        // - UploadTasks
        // - Files
        // - ShoppingLists
        // - Invites
    }
}
