using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for Tags.
/// </summary>
public class TagFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public Tag Vegetarisch { get; private set; } = null!;
    public Tag Vegan { get; private set; } = null!;
    public Tag Schnell { get; private set; } = null!;
    public Tag Einfach { get; private set; } = null!;
    public Tag LowCarb { get; private set; } = null!;

    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        var groups = Parent.GroupFixture;

        Vegetarisch = new Tag
        {
            GroupId = groups.FamilieMueller.Id,
            Name = "vegetarisch",
            Category = TagCategory.Diet
        };
        await Context.Tags.AddAsync(Vegetarisch, cancellationToken);

        Vegan = new Tag
        {
            GroupId = groups.FamilieMueller.Id,
            Name = "vegan",
            Category = TagCategory.Diet
        };
        await Context.Tags.AddAsync(Vegan, cancellationToken);

        Schnell = new Tag
        {
            GroupId = groups.FamilieMueller.Id,
            Name = "schnell",
            Category = TagCategory.Preparation
        };
        await Context.Tags.AddAsync(Schnell, cancellationToken);

        Einfach = new Tag
        {
            GroupId = groups.FamilieMueller.Id,
            Name = "einfach",
            Category = TagCategory.Preparation
        };
        await Context.Tags.AddAsync(Einfach, cancellationToken);

        LowCarb = new Tag
        {
            GroupId = groups.WGBerlin.Id,
            Name = "low-carb",
            Category = TagCategory.Diet
        };
        await Context.Tags.AddAsync(LowCarb, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken);
    }
}
