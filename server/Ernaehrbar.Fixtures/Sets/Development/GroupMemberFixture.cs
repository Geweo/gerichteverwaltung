using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for GroupMembers.
/// </summary>
public class GroupMemberFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public async Task AddUserToGroup(ApplicationDbContext context, User user, Group group, GroupRole role, CancellationToken cancellationToken)
    {
        // Prüfe ob GroupMember bereits existiert
        var existingMember = await context.GroupMembers
            .FirstOrDefaultAsync(m => m.UserId == user.Id && m.GroupId == group.Id, cancellationToken);
        
        if (existingMember != null)
        {
            // Aktualisiere Role falls nötig
            if (existingMember.Role != role)
            {
                existingMember.Role = role;
                await context.SaveChangesAsync(cancellationToken);
            }
            return;
        }

        var member = new GroupMember
        {
            UserId = user.Id,
            GroupId = group.Id,
            Role = role,
            JoinedAt = DateTime.UtcNow
        };
        await context.GroupMembers.AddAsync(member, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    protected override Task SeedAsync(CancellationToken cancellationToken)
    {
        // GroupMembers are created by UserFixture
        return Task.CompletedTask;
    }
}
