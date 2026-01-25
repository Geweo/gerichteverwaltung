namespace Ernaehrbar.Parts.Ports;

/// <summary>
/// Port for group repository operations (write).
/// </summary>
public interface IGroupRepository
{
    /// <summary>
    /// Gets a group by ID.
    /// </summary>
    Task<GroupDto?> GetByIdAsync(int groupId, CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO for group operations.
/// </summary>
public record GroupDto(
    int Id,
    string Name,
    string? Description = null
);
