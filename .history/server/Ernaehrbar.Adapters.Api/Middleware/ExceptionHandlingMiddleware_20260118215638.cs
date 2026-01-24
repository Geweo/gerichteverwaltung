using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ernaehrbar.Adapters.Api.Middleware;

/// <summary>
/// Zentrales Exception-Handling für die API. Fängt nicht behandelte Exceptions ab,
/// loggt sie und antwortet mit einem einheitlichen Fehlerformat (500).
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unbehandelte Exception: {Message}", ex.Message);
            await WriteErrorResponseAsync(context, ex);
        }
    }

    private async Task WriteErrorResponseAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        // ValidationException (FluentValidation) → 400 Bad Request
        if (ex is ValidationException validationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            
            var errors = validationException.Errors
                .Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
                .ToList();

            var body = JsonSerializer.Serialize(new { error = "Validierungsfehler", errors });
            await context.Response.WriteAsync(body);
            return;
        }

        // Alle anderen Exceptions → 500 Internal Server Error
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // In Development: echte Fehlermeldung; in Production: generische Meldung
        var message = _environment.IsDevelopment() || _environment.EnvironmentName == "Local"
            ? ex.Message
            : "Ein unerwarteter Fehler ist aufgetreten.";

        var body2 = JsonSerializer.Serialize(new { error = message });
        await context.Response.WriteAsync(body2);
    }
}

/// <summary>
/// Extension-Methoden zur Registrierung der Ernährbär-Middleware in der Pipeline.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Fügt die zentrale Exception-Handling-Middleware in die Pipeline ein.
    /// Sollte möglichst früh registriert werden, damit alle nachfolgenden Schritte abgefangen werden.
    /// </summary>
    public static IApplicationBuilder UseErnaehrbarExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
