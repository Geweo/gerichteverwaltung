using Ernaehrbar.Adapters.Api;
using Ernaehrbar.Adapters.Api.Controllers;
using Ernaehrbar.Adapters.Api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.IdentityModel.Tokens;
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

// Supabase JWT tokens use the auth endpoint as issuer, not the base URL
var supabaseIssuer = $"{supabaseUrl.TrimEnd('/')}/auth/v1";

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

// Swagger MUSS vor Authentication/Authorization kommen, damit Swagger-Routen ohne Auth funktionieren
// Swagger UI für Development und Local aktivieren
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ernährbär API v1");
        c.RoutePrefix = "swagger"; // Swagger UI at /swagger
    });
}
else
{
    // In Production: Nur Swagger JSON für API-Client-Generierung, kein UI
    app.UseSwagger();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Log startup information
Log.Information("🚀 Ernährbär API starting...");
var swaggerUrl = (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
    ? $"http://localhost:5000/swagger" 
    : "N/A";
Log.Information("📚 Swagger UI available at: {SwaggerUrl}", swaggerUrl);

app.Run();

