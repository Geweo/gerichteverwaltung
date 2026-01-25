using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Parts.Ports;
using Microsoft.EntityFrameworkCore;

namespace Ernaehrbar.Adapters.Infrastructure.ReadRepositories;

/// <summary>
/// Infrastructure-Adapter für IUserReadRepository: Read-optimierte User-Abfragen mit EF Core.
/// </summary>
public class UserReadRepository : IUserReadRepository
{
    private readonly ApplicationDbContext _context;

    public UserReadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<UserDto?> GetBySupabaseUserIdAsync(string supabaseUserId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.SupabaseUserId == supabaseUserId, cancellationToken);

        if (entity == null)
        {
            return null;
        }

        return new UserDto(
            Id: entity.Id,
            SupabaseUserId: entity.SupabaseUserId,
            Email: entity.Email,
            DisplayName: entity.DisplayName
        );
    }

    /// <inheritdoc />
    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (entity == null)
        {
            return null;
        }

        return new UserDto(
            Id: entity.Id,
            SupabaseUserId: entity.SupabaseUserId,
            Email: entity.Email,
            DisplayName: entity.DisplayName
        );
    }
}
