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
