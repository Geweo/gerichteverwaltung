using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for Users.
/// </summary>
public class UserFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public User MaxMueller { get; private set; } = null!;
    public User AnnaMueller { get; private set; } = null!;
    public User TomBerlin { get; private set; } = null!;
    public User LisaBerlin { get; private set; } = null!;
    public User SingleUser { get; private set; } = null!;

    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        var groups = Parent.GroupFixture;

        MaxMueller = new User
        {
            SupabaseUserId = "00000000-0000-0000-0000-000000000001",
            Email = "max.mueller@example.com",
            DisplayName = "Max Müller"
        };
        await Context.Users.AddAsync(MaxMueller, cancellationToken);

        AnnaMueller = new User
        {
            SupabaseUserId = "00000000-0000-0000-0000-000000000002",
            Email = "anna.mueller@example.com",
            DisplayName = "Anna Müller"
        };
        await Context.Users.AddAsync(AnnaMueller, cancellationToken);

        TomBerlin = new User
        {
            SupabaseUserId = "00000000-0000-0000-0000-000000000003",
            Email = "tom.berlin@example.com",
            DisplayName = "Tom Berlin"
        };
        await Context.Users.AddAsync(TomBerlin, cancellationToken);

        LisaBerlin = new User
        {
            SupabaseUserId = "00000000-0000-0000-0000-000000000004",
            Email = "lisa.berlin@example.com",
            DisplayName = "Lisa Berlin"
        };
        await Context.Users.AddAsync(LisaBerlin, cancellationToken);

        SingleUser = new User
        {
            SupabaseUserId = "00000000-0000-0000-0000-000000000005",
            Email = "single.user@example.com",
            DisplayName = "Single User"
        };
        await Context.Users.AddAsync(SingleUser, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken);

        // Add users to groups
        var groupMembers = Parent.GroupMemberFixture;
        await groupMembers.AddUserToGroup(MaxMueller, groups.FamilieMueller, GroupRole.Admin, cancellationToken);
        await groupMembers.AddUserToGroup(AnnaMueller, groups.FamilieMueller, GroupRole.Member, cancellationToken);
        await groupMembers.AddUserToGroup(TomBerlin, groups.WGBerlin, GroupRole.Admin, cancellationToken);
        await groupMembers.AddUserToGroup(LisaBerlin, groups.WGBerlin, GroupRole.Member, cancellationToken);
        await groupMembers.AddUserToGroup(SingleUser, groups.SingleUser, GroupRole.Admin, cancellationToken);
    }
}
