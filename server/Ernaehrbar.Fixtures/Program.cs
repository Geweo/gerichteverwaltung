using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Fixtures.Configuration;
using Ernaehrbar.Fixtures.Sets.Development;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Starting fixture loader...");

    var services = new ServiceCollection();
    services.AddSingleton<IConfiguration>(configuration);
    services.AddLogging(builder => builder.AddSerilog());

    // Add DbContext
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        Log.Error("Connection string 'DefaultConnection' not found in configuration");
        return 1;
    }

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));

    // Add fixtures
    services.AddErnaehrbarFixtures();

    var serviceProvider = services.BuildServiceProvider();

    // Get DbContext and DevelopmentFixtureSet
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var fixtureSet = scope.ServiceProvider.GetRequiredService<DevelopmentFixtureSet>();

    // Seed fixtures
    Log.Information("Loading fixtures into database...");
    await fixtureSet.Seed(dbContext, CancellationToken.None);
    Log.Information("✅ Fixtures loaded successfully!");

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to load fixtures");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
