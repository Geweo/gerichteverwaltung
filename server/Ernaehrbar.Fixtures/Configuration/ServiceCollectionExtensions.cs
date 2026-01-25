using Ernaehrbar.Fixtures.Sets.Development;
using Microsoft.Extensions.DependencyInjection;

namespace Ernaehrbar.Fixtures.Configuration;

/// <summary>
/// Extension methods for configuring fixtures in dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Ernährbär fixtures to the service collection.
    /// </summary>
    public static IServiceCollection AddErnaehrbarFixtures(this IServiceCollection services)
    {
        services.AddSingleton<DevelopmentFixtureSet>();
        return services;
    }
}
