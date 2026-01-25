using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Parts.Ports;
using Microsoft.EntityFrameworkCore;

namespace Ernaehrbar.Adapters.Infrastructure.Repositories;

/// <summary>
/// Infrastructure adapter implementing IUserRepository port.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

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

    public async Task<UserDto> CreateUserAsync(string supabaseUserId, string email, CancellationToken cancellationToken = default)
    {
        var entity = new User
        {
            SupabaseUserId = supabaseUserId,
            Email = email,
        };

        await _context.Users.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new UserDto(
            Id: entity.Id,
            SupabaseUserId: entity.SupabaseUserId,
            Email: entity.Email,
            DisplayName: entity.DisplayName
        );
    }
}
