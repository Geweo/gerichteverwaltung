using System.Net.Http.Headers;
using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Fixtures.Sets.Development;
using Ernaehrbar.Tests.Integration.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Ernaehrbar.Tests.Integration;

/// <summary>
/// Basis-Klasse für Integration-Tests (E2E-Tests).
/// </summary>
[Collection("Integration")]
[Trait("Category", "Integration")]
public class BaseE2ETest : IClassFixture<CustomWebApplicationFactory<global::Program>>, IAsyncLifetime
{
    private static int _classExecutionCounter;
    private readonly AsyncServiceScope _scope;

    // JWT-Konfiguration für Tests (entspricht lokaler Supabase)
    protected const string JwtSupabaseIssuer = "http://127.0.0.1:54321/auth/v1";
    protected const string JwtSupabaseSecret = "super-secret-jwt-token-with-at-least-32-characters-long";

    protected readonly ApplicationDbContext DbContext;
    protected readonly CustomWebApplicationFactory<global::Program> Factory;
    protected readonly SupabaseJwtUtility JwtValid;
    protected readonly ITestOutputHelper Output;
    protected readonly TestFixtures TestFixtures;
    protected readonly DevelopmentFixtureSet Fixtures;

    protected string UniqueExecutionId { get; private set; }

    public BaseE2ETest(ITestOutputHelper output, CustomWebApplicationFactory<global::Program> factory)
    {
        Output = output;
        Factory = factory;
        factory.Output = output;

        _scope = Factory.Services.CreateAsyncScope();
        DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        TestFixtures = _scope.ServiceProvider.GetRequiredService<TestFixtures>();
        Fixtures = _scope.ServiceProvider.GetRequiredService<DevelopmentFixtureSet>();

        JwtValid = new SupabaseJwtUtility(JwtSupabaseIssuer, JwtSupabaseSecret);

        Interlocked.Increment(ref _classExecutionCounter);
        UniqueExecutionId = $"id-{_classExecutionCounter}";

        Log.Logger = new LoggerConfiguration()
            .WriteTo.TestOutput(output)
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .CreateBootstrapLogger();
    }

    public async Task InitializeAsync()
    {
        Output.WriteLine($"InitializeAsync {Factory.DatabaseName} {DateTime.Now.ToLongTimeString()}");
        await Factory.RecreateDatabase();
    }

    public async Task DisposeAsync()
    {
        Output.WriteLine($"DisposeAsync {Factory.DatabaseName} {DateTime.Now.ToLongTimeString()}");

        // Scope schließen (DbContext-Verbindungen schließen)
        await _scope.DisposeAsync();

        // Npgsql Connection Pool leeren
        NpgsqlConnection.ClearAllPools();

        // Datenbank löschen
        await using var scope = Factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
    }

    protected SupabaseJwtContent CreateSupabaseJwtContent(string email, Guid? userId = null, bool emailVerified = true)
    {
        return new SupabaseJwtContent
        {
            UserId = userId ?? Guid.NewGuid(),
            Email = email,
            AppMetadata = new SupabaseJwtContentAppMetadata
            {
                Provider = "email",
                Providers = ["email"]
            },
            UserMetadata = new SupabaseJwtContentUserMetadata
            {
                EmailVerified = emailVerified
            }
        };
    }

    /// <summary>
    /// Erstellt einen gültigen Supabase JWT-Token für Tests.
    /// </summary>
    protected string CreateSupabaseJwtToken(string email, Guid? userId = null, bool emailVerified = true)
    {
        var tokenContent = CreateSupabaseJwtContent(email, userId, emailVerified);
        return JwtValid.ToJwtTokenString(tokenContent);
    }

    /// <summary>
    /// Erstellt einen HttpClient mit Authorization-Header.
    /// </summary>
    protected HttpClient GetAuthenticatedClient(string email, Guid? userId = null, bool emailVerified = true)
    {
        var client = Factory.CreateClient();
        var token = CreateSupabaseJwtToken(email, userId, emailVerified);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    /// <summary>
    /// Erstellt einen HttpClient mit Authorization-Header für einen Fixture-User.
    /// </summary>
    protected HttpClient GetAuthenticatedClientForUser(Ernaehrbar.Adapters.Infrastructure.Data.Entities.User user)
    {
        var supabaseUserId = Guid.Parse(user.SupabaseUserId);
        return GetAuthenticatedClient(user.Email, supabaseUserId);
    }
}
