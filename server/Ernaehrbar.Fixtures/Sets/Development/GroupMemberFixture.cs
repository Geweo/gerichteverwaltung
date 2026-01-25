using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for GroupMembers.
/// </summary>
public class GroupMemberFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public async Task AddUserToGroup(User user, Group group, GroupRole role, CancellationToken cancellationToken)
    {
        var member = new GroupMember
        {
            UserId = user.Id,
            GroupId = group.Id,
            Role = role,
            JoinedAt = DateTime.UtcNow
        };
        await Context.GroupMembers.AddAsync(member, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    protected override Task SeedAsync(CancellationToken cancellationToken)
    {
        // GroupMembers are created by UserFixture
        return Task.CompletedTask;
    }
}
