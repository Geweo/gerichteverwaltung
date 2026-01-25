namespace Ernaehrbar.Tests.Integration.TestHelpers;

/// <summary>
/// JWT Content structure for Supabase tokens (simplified for tests).
/// </summary>
public class SupabaseJwtContent
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = "";
    public SupabaseJwtContentAppMetadata AppMetadata { get; init; } = new();
    public SupabaseJwtContentUserMetadata UserMetadata { get; init; } = new();
}

public class SupabaseJwtContentAppMetadata
{
    public string Provider { get; init; } = "email";
    public List<string> Providers { get; init; } = ["email"];
}

public class SupabaseJwtContentUserMetadata
{
    public bool EmailVerified { get; init; } = true;
}
