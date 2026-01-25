using System.Security.Claims;

namespace Ernaehrbar.Adapters.Api.Utilities;

/// <summary>
/// Utility for extracting Supabase JWT content from ClaimsPrincipal.
/// Used by SecurityMiddleware to map Supabase users to local database users.
/// </summary>
public class SupabaseJwtUtility
{
    /// <summary>
    /// Extracts Supabase user information from ClaimsPrincipal.
    /// </summary>
    /// <param name="principal">The authenticated ClaimsPrincipal from JWT token.</param>
    /// <returns>SupabaseJwtContent with UserId and Email, or null if parsing fails.</returns>
    public SupabaseJwtContent? ToSupabaseJwtContent(ClaimsPrincipal? principal)
    {
        if (principal == null) return null;

        // Supabase stores the user ID in "sub" claim or NameIdentifier
        var userIdString = principal.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? principal.FindFirstValue("sub");
        
        if (string.IsNullOrEmpty(userIdString))
        {
            return null;
        }

        // Email is stored in Email claim
        var email = principal.FindFirstValue(ClaimTypes.Email);
        
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        return new SupabaseJwtContent
        {
            UserId = userIdString,
            Email = email,
        };
    }
}
