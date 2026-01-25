using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for RecipeRating entity.
/// </summary>
public class RecipeRatingEntityMap : BaseMap<RecipeRating>
{
    public override void Configure(EntityTypeBuilder<RecipeRating> entity)
    {
        base.Configure(entity);

        entity.ToTable("RecipeRatings");

        entity.Property(e => e.Rating)
            .HasComment("Rating from 1 to 5 (null if not rated)");

        entity.Property(e => e.IsFavorite)
            .IsRequired()
            .HasDefaultValue(false);

        entity.Property(e => e.Comment)
            .HasMaxLength(1000);

        // Composite unique constraint (User + Recipe)
        entity.HasIndex(rr => new { rr.UserId, rr.RecipeId })
            .IsUnique();

        // Recipe relationship
        entity.HasOne(rr => rr.Recipe)
            .WithMany(r => r.Ratings)
            .HasForeignKey(rr => rr.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // User relationship
        entity.HasOne(rr => rr.User)
            .WithMany()
            .HasForeignKey(rr => rr.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
