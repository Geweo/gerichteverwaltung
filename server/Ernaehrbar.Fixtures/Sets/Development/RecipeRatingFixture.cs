using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for RecipeRatings.
/// </summary>
public class RecipeRatingFixture : SeedableFixture<DevelopmentFixtureSet>
{
    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        var recipes = Parent.RecipeFixture;
        var users = Parent.UserFixture;

        var rating1 = new RecipeRating
        {
            UserId = users.MaxMueller.Id,
            RecipeId = recipes.SpaghettiBolognese.Id,
            Rating = 5,
            Comment = "Sehr lecker!"
        };
        await Context.RecipeRatings.AddAsync(rating1, cancellationToken);

        var rating2 = new RecipeRating
        {
            UserId = users.AnnaMueller.Id,
            RecipeId = recipes.SpaghettiBolognese.Id,
            Rating = 4,
            Comment = "Gut, aber etwas zu salzig"
        };
        await Context.RecipeRatings.AddAsync(rating2, cancellationToken);

        var rating3 = new RecipeRating
        {
            UserId = users.TomBerlin.Id,
            RecipeId = recipes.CaesarSalad.Id,
            Rating = 5
        };
        await Context.RecipeRatings.AddAsync(rating3, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken);
    }
}
