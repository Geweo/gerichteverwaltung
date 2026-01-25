using System.Net;
using System.Text.Json;
using Ernaehrbar.Parts.Exceptions;
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
        context.Response.ContentType = "text/plain";

        // Handle specific Ernährbär exceptions
        var (statusCode, message) = ex switch
        {
            ErnaehrbarUnauthorizedException => (HttpStatusCode.Unauthorized, ex.Message ?? "Unauthorized"),
            ErnaehrbarNotFoundException => (HttpStatusCode.NotFound, ex.Message ?? "Not Found"),
            ErnaehrbarValidationException => (HttpStatusCode.BadRequest, ex.Message ?? "Validation Error"),
            ValidationException validationException => (HttpStatusCode.BadRequest, GetValidationErrorMessage(validationException)),
            _ => (HttpStatusCode.InternalServerError, GetGenericErrorMessage(ex))
        };

        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(message);
    }

    private string GetValidationErrorMessage(ValidationException validationException)
    {
        var errors = validationException.Errors
            .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
            .ToList();

        return string.Join("; ", errors);
    }

    private string GetGenericErrorMessage(Exception ex)
    {
        // In Development: echte Fehlermeldung; in Production: generische Meldung
        return _environment.IsDevelopment() || _environment.EnvironmentName == "Local"
            ? ex.Message
            : "Ein unerwarteter Fehler ist aufgetreten.";
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
