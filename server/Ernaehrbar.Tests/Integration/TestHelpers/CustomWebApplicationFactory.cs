using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Fixtures.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

namespace Ernaehrbar.Tests.Integration.TestHelpers;

/// <summary>
/// Factory für Integration-Tests mit eigener Test-Datenbank.
/// </summary>
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private readonly string _databaseId = Guid.NewGuid().ToString().Replace("-", "");

    public ITestOutputHelper? Output { get; set; }

    /// <summary>
    /// Der Datenbankname für diesen Test.
    /// </summary>
    public string DatabaseName => $"ernaehrbar-test-{_databaseId}";

    /// <summary>
    /// Erstellt die Test-Datenbank neu und lädt Fixtures.
    /// </summary>
    public async Task RecreateDatabase()
    {
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        // Load fixtures
        var fixtureSet = scope.ServiceProvider.GetRequiredService<Ernaehrbar.Fixtures.Sets.Development.DevelopmentFixtureSet>();
        await fixtureSet.Seed(dbContext, CancellationToken.None);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.UseSerilog((_, _, config) =>
        {
            config.MinimumLevel.Information();
            config.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
            if (Output != null)
            {
                config.WriteTo.TestOutput(Output);
            }
        });

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        var connectionString = $"Host=localhost;Database={DatabaseName};Username=postgres;Password=postgres;Port=5432;Pooling=false;";

        // Logging wird bereits über Serilog konfiguriert (siehe CreateHost)

        builder.ConfigureAppConfiguration(config =>
        {
            // Test-Konfiguration: JWT Secret für HS256
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Supabase:Url"] = "http://127.0.0.1:54321",
                ["Supabase:JwksUrl"] = "http://127.0.0.1:54321/auth/v1/.well-known/jwks.json",
                ["Supabase:JwtSecret"] = "super-secret-jwt-token-with-at-least-32-characters-long",
                ["ConnectionStrings:DefaultConnection"] = connectionString
            });
        });

        builder.ConfigureTestServices(services =>
        {
            // Entferne die normale DbContext-Registrierung
            var dbContextDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // Füge Test-DbContext hinzu
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            }, ServiceLifetime.Scoped, ServiceLifetime.Singleton);

            // TestFixtures für Test-Daten (für manuelle Erstellung)
            services.AddSingleton<TestFixtures>();

            // Development Fixtures für Test-Daten
            services.AddErnaehrbarFixtures();
        });
    }
}
