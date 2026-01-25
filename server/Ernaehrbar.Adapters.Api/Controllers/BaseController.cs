using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Ernaehrbar.Adapters.Api.Controllers;

/// <summary>
/// Base controller for API endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Extrahiert die Supabase User ID (sub) aus dem JWT Token.
    /// </summary>
    protected string? GetSupabaseUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
    }

    /// <summary>
    /// Extrahiert die lokale User ID aus dem SecurityMiddleware.
    /// Diese wird automatisch vom SecurityMiddleware gesetzt, wenn der User authentifiziert ist.
    /// </summary>
    protected int? GetUserId()
    {
        if (HttpContext.Items.TryGetValue("UserId", out var userId) && userId is int id)
        {
            return id;
        }
        return null;
    }

    /// <summary>
    /// Extrahiert die Email-Adresse des aktuellen Users aus dem SecurityMiddleware.
    /// </summary>
    protected string? GetUserEmail()
    {
        if (HttpContext.Items.TryGetValue("UserEmail", out var email) && email is string emailString)
        {
            return emailString;
        }
        return null;
    }

    /// <summary>
    /// Extrahiert die Group ID aus dem aktuellen User.
    /// TODO: Implementieren, wenn User-Group-Mapping vorhanden ist.
    /// Für jetzt: GroupId muss als Parameter übergeben werden.
    /// </summary>
    protected int? GetGroupId()
    {
        // TODO: Implementieren, wenn User-Group-Mapping vorhanden ist
        // Aktuell muss GroupId als Parameter übergeben werden
        return null;
    }
}

