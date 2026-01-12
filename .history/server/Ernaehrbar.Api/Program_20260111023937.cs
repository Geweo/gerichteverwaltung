using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.LLM;
using Ernaehrbar.Parts.Ports;
using Ernaehrbar.Parts.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
        if (builder.Environment.IsDevelopment())
        {
            // In Development: Allow all origins for easier development
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

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ernährbär API",
        Version = "v1",
        Description = "API für den Rezept- & Zutatenplaner mit Bring-Anbindung"
    });
    
    // Add JWT authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add Entity Framework Core with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register HTTP Client for LLM services
builder.Services.AddHttpClient();

// Register LLM Service based on configuration
builder.Services.AddSingleton<ILLMService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    var configuration = sp.GetRequiredService<IConfiguration>();
    var provider = configuration["LLM:Provider"] ?? "Ollama";

    return provider switch
    {
        "Ollama" => CreateOllamaAdapter(httpClient, configuration),
        // Future providers can be added here:
        // "OpenAI" => CreateOpenAIAdapter(httpClient, configuration),
        // "Anthropic" => CreateAnthropicAdapter(httpClient, configuration),
        _ => CreateOllamaAdapter(httpClient, configuration)
    };
});

static ILLMService CreateOllamaAdapter(HttpClient httpClient, IConfiguration configuration)
{
    var ollamaUrl = configuration["LLM:Ollama:Url"] 
        ?? configuration["Ollama:Url"] // Fallback for old config format
        ?? "http://localhost:11434";
    var modelName = configuration["LLM:Ollama:ModelName"] 
        ?? configuration["Ollama:ModelName"] // Fallback for old config format
        ?? "llama3.2";
    
    return new OllamaAdapter(httpClient, ollamaUrl, modelName);
}

// Register Use Cases
builder.Services.AddScoped<GeneratePlan>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ernährbär API v1");
        options.RoutePrefix = "swagger"; // Swagger UI at /swagger
    });
}

// Always enable Swagger JSON endpoint for API client generation
app.UseSwagger();

// Only use HTTPS redirection in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS must be before Authentication/Authorization
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Log startup information
Log.Information("🚀 Ernährbär API starting...");
var swaggerUrl = app.Environment.IsDevelopment() 
    ? $"http://localhost:5000/swagger" 
    : "N/A";
Log.Information("📚 Swagger UI available at: {SwaggerUrl}", swaggerUrl);

app.Run();

