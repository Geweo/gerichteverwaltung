using Ernaehrbar.Adapters.Infrastructure.Bring;
using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.LLM;
using Ernaehrbar.Adapters.Infrastructure.ReadRepositories;
using Ernaehrbar.Adapters.Infrastructure.Storage;
using Ernaehrbar.Parts.Ports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ernaehrbar.Adapters.Infrastructure;

/// <summary>
/// Erweiterungsmethoden für die Registrierung der Infrastructure-Adapter (Port-Implementierungen).
/// Entspricht der hexagonalen Architektur: Adapters.Infrastructure implementiert die Ports aus Parts.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registriert alle Infrastructure-Adapter: DbContext, IRecipeStorage, IBringExporter, ILLMService.
    /// </summary>
    /// <param name="services">Die Service-Collection.</param>
    /// <param name="configuration">Konfiguration für Connection Strings und LLM-Einstellungen.</param>
    /// <returns>Die Service-Collection zur Verkettung.</returns>
    public static IServiceCollection AddErnaehrbarInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Entity Framework Core mit PostgreSQL
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // HTTP-Client für LLM-Adapter (z. B. Ollama)
        services.AddHttpClient();

        // Port-Implementierungen (Write)
        services.AddScoped<IRecipeStorage, RecipeStorageAdapter>();
        services.AddScoped<IBringExporter, BringExporterAdapter>();

        // Read-Repositories (Read-optimiert)
        services.AddScoped<IRecipeReadRepository, RecipeReadRepository>();
        services.AddScoped<IMealPlanReadRepository, MealPlanReadRepository>();

        // ILLMService: Ollama oder andere Provider je nach Konfiguration
        services.AddSingleton<ILLMService>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            var config = sp.GetRequiredService<IConfiguration>();
            var provider = config["LLM:Provider"] ?? "Ollama";

            return provider switch
            {
                "Ollama" => CreateOllamaAdapter(httpClient, config),
                _ => CreateOllamaAdapter(httpClient, config)
            };
        });

        return services;
    }

    private static ILLMService CreateOllamaAdapter(HttpClient httpClient, IConfiguration configuration)
    {
        var ollamaUrl = configuration["LLM:Ollama:Url"]
            ?? configuration["Ollama:Url"]
            ?? "http://localhost:11434";
        var modelName = configuration["LLM:Ollama:ModelName"]
            ?? configuration["Ollama:ModelName"]
            ?? "llama3.2";

        return new OllamaAdapter(httpClient, ollamaUrl, modelName);
    }
}
