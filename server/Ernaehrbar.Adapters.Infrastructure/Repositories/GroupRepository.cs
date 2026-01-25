using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Parts.Ports;
using Microsoft.EntityFrameworkCore;

namespace Ernaehrbar.Adapters.Infrastructure.Repositories;

/// <summary>
/// Infrastructure adapter implementing IGroupRepository port.
/// </summary>
public class GroupRepository : IGroupRepository
{
    private readonly ApplicationDbContext _context;

    public GroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GroupDto?> GetByIdAsync(int groupId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Groups
            .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);

        if (entity == null)
        {
            return null;
        }

        return new GroupDto(
            Id: entity.Id,
            Name: entity.Name,
            Description: entity.Description
        );
    }
}
