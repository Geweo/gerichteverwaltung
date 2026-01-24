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
    protected string? GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
    }

    /// <summary>
    /// Extrahiert die Group ID aus dem aktuellen User.
    /// TODO: Implementieren, wenn User-Middleware vorhanden ist.
    /// Für jetzt: GroupId muss als Parameter übergeben werden.
    /// </summary>
    protected int? GetGroupId()
    {
        // TODO: Implementieren, wenn User-Synchronisation vorhanden ist
        // Aktuell muss GroupId als Parameter übergeben werden
        return null;
    }
}

