using Ernaehrbar.Adapters.Infrastructure;
using Ernaehrbar.Adapters.Api.Middleware;
using Ernaehrbar.Parts.Handlers;
using Ernaehrbar.Parts.Validation;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Ernaehrbar.Adapters.Api;

/// <summary>
/// Erweiterungsmethoden für die Registrierung der API-Schicht und Anbindung der Infrastructure.
/// Entspricht der hexagonalen Architektur: Adapters.Api hängt von Parts (Commands/Handlers) und
/// Adapters.Infrastructure (Port-Implementierungen) ab und verknüpft sie.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registriert Infrastructure-Adapter, MediatR (Commands/Handlers), Swagger und API-Explorer.
    /// Controllers und ApplicationPart werden in der Host-Application (Api) registriert.
    /// </summary>
    /// <param name="services">Die Service-Collection.</param>
    /// <param name="configuration">Konfiguration.</param>
    /// <returns>Die Service-Collection zur Verkettung.</returns>
    public static IServiceCollection AddErnaehrbarApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Infrastructure: Port-Implementierungen (DbContext, IRecipeStorage, IBringExporter, ILLMService)
        services.AddErnaehrbarInfrastructure(configuration);

        // FluentValidation: Command-Validatoren aus Parts
        services.AddValidatorsFromAssembly(typeof(GenerateRecipesCommandValidator).Assembly);

        // MediatR: Commands/Handlers aus Parts (mit FluentValidation-Integration)
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(GenerateRecipesCommandHandler).Assembly);
            cfg.AddOpenBehavior(typeof(FluentValidationBehavior<,>));
        });

        // API Explorer und Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ernährbär API",
                Version = "v1",
                Description = "API für den Rezept- & Zutatenplaner mit Bring-Anbindung"
            });

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

        return services;
    }
}
