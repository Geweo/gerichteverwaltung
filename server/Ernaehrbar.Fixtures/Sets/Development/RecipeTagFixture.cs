using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for RecipeTags.
/// </summary>
public class RecipeTagFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public async Task AddTag(Recipe recipe, Tag tag, CancellationToken cancellationToken)
    {
        var recipeTag = new RecipeTag
        {
            RecipeId = recipe.Id,
            TagId = tag.Id
        };
        await Context.RecipeTags.AddAsync(recipeTag, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    protected override Task SeedAsync(CancellationToken cancellationToken)
    {
        // RecipeTags are created by RecipeFixture
        return Task.CompletedTask;
    }
}
