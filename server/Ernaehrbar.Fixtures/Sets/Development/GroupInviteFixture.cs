using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for GroupInvites.
/// </summary>
public class GroupInviteFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public GroupInvite ActiveInvite { get; private set; } = null!;
    public GroupInvite ExpiredInvite { get; private set; } = null!;

    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        var groups = Parent.GroupFixture;
        var users = Parent.UserFixture;

        ActiveInvite = new GroupInvite
        {
            GroupId = groups.FamilieMueller.Id,
            Token = Guid.NewGuid().ToString(),
            InvitedEmail = "invited@example.com",
            CreatedByUserId = users.MaxMueller.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsUsed = false
        };
        await Context.GroupInvites.AddAsync(ActiveInvite, cancellationToken);

        ExpiredInvite = new GroupInvite
        {
            GroupId = groups.WGBerlin.Id,
            Token = Guid.NewGuid().ToString(),
            InvitedEmail = "expired@example.com",
            CreatedByUserId = users.TomBerlin.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(-1),
            IsUsed = false
        };
        await Context.GroupInvites.AddAsync(ExpiredInvite, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken);
    }
}
