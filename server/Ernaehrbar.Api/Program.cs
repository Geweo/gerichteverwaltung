using Ernaehrbar.Adapters.Api;
using Ernaehrbar.Adapters.Api.Controllers;
using Ernaehrbar.Adapters.Api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Configure Supabase JWT Authentication
var supabaseUrl = builder.Configuration["Supabase:Url"] ?? throw new InvalidOperationException("Supabase:Url is not configured");
var jwksUrl = builder.Configuration["Supabase:JwksUrl"] ?? throw new InvalidOperationException("Supabase:JwksUrl is not configured");
var jwtSecret = builder.Configuration["Supabase:JwtSecret"];

// Supabase JWT tokens use the auth endpoint as issuer, not the base URL
var supabaseIssuer = $"{supabaseUrl.TrimEnd('/')}/auth/v1";

// Für Tests: Wenn JwtSecret gesetzt ist, verwende HS256 (symmetric), sonst JWKS (asymmetric)
var useJwtSecret = !string.IsNullOrEmpty(jwtSecret) && jwtSecret.Length >= 32;

// Cache for JWKS to avoid fetching on every request
var jwksCache = new System.Collections.Concurrent.ConcurrentDictionary<string, Microsoft.IdentityModel.Tokens.JsonWebKeySet>();
var jwksHttpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = supabaseIssuer;
        
        // Für lokale Entwicklung: HTTPS-Metadaten-Anforderung deaktivieren
        // (lokale Supabase läuft auf HTTP)
        if (builder.Environment.IsDevelopment() || builder.Environment.EnvironmentName == "Local")
        {
            options.RequireHttpsMetadata = false;
        }
        
        if (useJwtSecret)
        {
            // HS256 (symmetric) für lokale Entwicklung/Tests
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = supabaseIssuer,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(jwtSecret!)),
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        }
        else
        {
            // ES256 (asymmetric) mit JWKS für Cloud
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                {
                    try
                    {
                        // Use cached JWKS or fetch from endpoint
                        var jwks = jwksCache.GetOrAdd(jwksUrl, url =>
                        {
                            Log.Information("Fetching JWKS from {JwksUrl}", url);
                            var response = jwksHttpClient.GetStringAsync(url).GetAwaiter().GetResult();
                            return new Microsoft.IdentityModel.Tokens.JsonWebKeySet(response);
                        });
                        
                        return jwks.GetSigningKeys();
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex, "Failed to fetch JWKS from {JwksUrl}. Error: {Error}", jwksUrl, ex.Message);
                        throw;
                    }
                },
                ValidIssuer = supabaseIssuer
            };
        }
    });

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (builder.Environment.IsDevelopment() || builder.Environment.EnvironmentName == "Local")
        {
            // In Development/Local: Allow all origins for easier development
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            // In Production: Restrict to specific origins
            policy.WithOrigins(
                    builder.Configuration["AllowedOrigins"]?.Split(',') 
                    ?? new[] { "https://your-production-domain.com" })
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
    });
});

// Add services: Infrastructure, Use Cases, Swagger (hexagonale Architektur)
builder.Services.AddErnaehrbarApi(builder.Configuration);

// Controllers: ApplicationPart, damit Controller aus Ernaehrbar.Adapters.Api geladen werden
builder.Services.AddControllers()
    .AddApplicationPart(typeof(RecipesController).Assembly);

var app = builder.Build();

// Exception-Handling zuerst, damit alle nachfolgenden Schritte abgefangen werden
app.UseErnaehrbarExceptionHandling();

// Only use HTTPS redirection in production
if (!app.Environment.IsDevelopment() && app.Environment.EnvironmentName != "Local")
{
    app.UseHttpsRedirection();
}

// CORS must be before Authentication/Authorization
app.UseCors();

// OpenAPI & Scalar MUSS vor Authentication/Authorization kommen, damit Routen ohne Auth funktionieren
// Scalar UI für Development und Local aktivieren
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.MapOpenApi();
    
    app.MapScalarApiReference(opts =>
    {
        opts.DynamicBaseServerUrl = true;
        opts.WithTitle("Ernährbär API");
        opts.EnableDarkMode();
        opts.WithTheme(Scalar.AspNetCore.ScalarTheme.BluePlanet);
        opts.ShowOperationId = true;
        opts.AddServer("http://localhost:5000/api/");
        opts.AddPreferredSecuritySchemes("Bearer");
    });
    
    Log.Information("📚 Scalar UI available at: http://localhost:5000/scalar");
    Log.Information("📄 OpenAPI JSON available at: http://localhost:5000/openapi/v1.json");
}
else
{
    // In Production: Nur OpenAPI JSON für API-Client-Generierung, kein UI
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Log startup information
Log.Information("🚀 Ernährbär API starting...");

app.Run();

// Öffentliche Program-Klasse für Tests (Top-Level Statements erzeugen eine interne Klasse im globalen Namespace)
// Diese partial class macht sie öffentlich zugänglich
public partial class Program { }

