using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for Groups.
/// </summary>
public class GroupFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public Group FamilieMueller { get; private set; } = null!;
    public Group WGBerlin { get; private set; } = null!;
    public Group SingleUser { get; private set; } = null!;

    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        FamilieMueller = new Group
        {
            Name = "Familie Müller",
            Description = "Familie mit 2 Kindern"
        };
        await Context.Groups.AddAsync(FamilieMueller, cancellationToken);

        WGBerlin = new Group
        {
            Name = "WG Berlin",
            Description = "Wohngemeinschaft in Berlin"
        };
        await Context.Groups.AddAsync(WGBerlin, cancellationToken);

        SingleUser = new Group
        {
            Name = "Single User",
            Description = "Einzelperson"
        };
        await Context.Groups.AddAsync(SingleUser, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken);
    }
}
