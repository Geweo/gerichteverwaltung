using Ernaehrbar.Adapters.Infrastructure.Data;
using Serilog;

namespace Ernaehrbar.Fixtures.Utilities;

/// <summary>
/// Base class for seedable fixtures without parent dependency.
/// </summary>
public abstract class SeedableFixture
{
    private bool _isSeeded;

    protected ApplicationDbContext Context { get; private set; } = null!;

    protected async Task SeedInternal(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        Context = context;
        if (_isSeeded)
        {
            Log.Logger.Warning("Fixture {Name} has already been seeded, skipping", GetType().Name);
            return;
        }

        _isSeeded = true;
        Log.Logger.Debug("Seeding {FixtureType}", GetType().FullName);
        var startTime = DateTime.UtcNow;
        await SeedAsync(cancellationToken);
        var duration = DateTime.UtcNow - startTime;
        Log.Logger.Debug("Seeded {FixtureType} in {Duration}ms", GetType().FullName, duration.TotalMilliseconds);
    }

    protected abstract Task SeedAsync(CancellationToken cancellationToken);

    public async Task Seed(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        await SeedInternal(context, cancellationToken);
    }
}

/// <summary>
/// Base class for seedable fixtures with parent dependency.
/// </summary>
public abstract class SeedableFixture<TParent> : SeedableFixture
{
    protected TParent Parent { get; private set; } = default!;

    public async Task Seed(ApplicationDbContext context, TParent parent, CancellationToken cancellationToken)
    {
        Parent = parent;
        await SeedInternal(context, cancellationToken);
    }
}
