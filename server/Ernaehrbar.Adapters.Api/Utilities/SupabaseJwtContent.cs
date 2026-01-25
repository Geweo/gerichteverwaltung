namespace Ernaehrbar.Adapters.Api.Utilities;

/// <summary>
/// JWT Content structure for Supabase tokens.
/// Extracted from ClaimsPrincipal for user mapping.
/// </summary>
public class SupabaseJwtContent
{
    public string UserId { get; init; } = "";
    public string Email { get; init; } = "";
}
