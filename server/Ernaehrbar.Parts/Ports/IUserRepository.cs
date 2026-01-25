namespace Ernaehrbar.Parts.Ports;

/// <summary>
/// Port for user repository operations (write).
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    Task<UserDto?> GetByIdAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user from Supabase authentication.
    /// Automatically called by SecurityMiddleware when a Supabase user logs in for the first time.
    /// </summary>
    Task<UserDto> CreateUserAsync(string supabaseUserId, string email, CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO for user operations.
/// </summary>
public record UserDto(
    int Id,
    string SupabaseUserId,
    string Email,
    string? DisplayName = null
);
