namespace Ernaehrbar.Parts.Ports;

/// <summary>
/// Port für Read-optimierte User-Abfragen.
/// Separate von IUserRepository (Write-Operations).
/// </summary>
public interface IUserReadRepository
{
    /// <summary>
    /// Ruft einen User anhand der Supabase User ID ab.
    /// </summary>
    Task<UserDto?> GetBySupabaseUserIdAsync(string supabaseUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ruft einen User anhand der Email-Adresse ab.
    /// </summary>
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
