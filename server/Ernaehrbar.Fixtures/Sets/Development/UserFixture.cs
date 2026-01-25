using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;
using Microsoft.EntityFrameworkCore;

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

        // Prüfe ob User bereits existiert, wenn ja, lade ihn
        MaxMueller = await Context.Users.FirstOrDefaultAsync(u => u.SupabaseUserId == "00000000-0000-0000-0000-000000000001", cancellationToken);
        if (MaxMueller == null)
        {
            MaxMueller = new User
            {
                SupabaseUserId = "00000000-0000-0000-0000-000000000001",
                Email = "max.mueller@example.com",
                DisplayName = "Max Müller"
            };
            await Context.Users.AddAsync(MaxMueller, cancellationToken);
        }

        AnnaMueller = await Context.Users.FirstOrDefaultAsync(u => u.SupabaseUserId == "00000000-0000-0000-0000-000000000002", cancellationToken);
        if (AnnaMueller == null)
        {
            AnnaMueller = new User
            {
                SupabaseUserId = "00000000-0000-0000-0000-000000000002",
                Email = "anna.mueller@example.com",
                DisplayName = "Anna Müller"
            };
            await Context.Users.AddAsync(AnnaMueller, cancellationToken);
        }

        TomBerlin = await Context.Users.FirstOrDefaultAsync(u => u.SupabaseUserId == "00000000-0000-0000-0000-000000000003", cancellationToken);
        if (TomBerlin == null)
        {
            TomBerlin = new User
            {
                SupabaseUserId = "00000000-0000-0000-0000-000000000003",
                Email = "tom.berlin@example.com",
                DisplayName = "Tom Berlin"
            };
            await Context.Users.AddAsync(TomBerlin, cancellationToken);
        }

        LisaBerlin = await Context.Users.FirstOrDefaultAsync(u => u.SupabaseUserId == "00000000-0000-0000-0000-000000000004", cancellationToken);
        if (LisaBerlin == null)
        {
            LisaBerlin = new User
            {
                SupabaseUserId = "00000000-0000-0000-0000-000000000004",
                Email = "lisa.berlin@example.com",
                DisplayName = "Lisa Berlin"
            };
            await Context.Users.AddAsync(LisaBerlin, cancellationToken);
        }

        SingleUser = await Context.Users.FirstOrDefaultAsync(u => u.SupabaseUserId == "00000000-0000-0000-0000-000000000005", cancellationToken);
        if (SingleUser == null)
        {
            SingleUser = new User
            {
                SupabaseUserId = "00000000-0000-0000-0000-000000000005",
                Email = "single.user@example.com",
                DisplayName = "Single User"
            };
            await Context.Users.AddAsync(SingleUser, cancellationToken);
        }

        await Context.SaveChangesAsync(cancellationToken);

        // Add users to groups
        var groupMembers = Parent.GroupMemberFixture;
        await groupMembers.AddUserToGroup(Context, MaxMueller, groups.FamilieMueller, GroupRole.Admin, cancellationToken);
        await groupMembers.AddUserToGroup(Context, AnnaMueller, groups.FamilieMueller, GroupRole.Member, cancellationToken);
        await groupMembers.AddUserToGroup(Context, TomBerlin, groups.WGBerlin, GroupRole.Admin, cancellationToken);
        await groupMembers.AddUserToGroup(Context, LisaBerlin, groups.WGBerlin, GroupRole.Member, cancellationToken);
        await groupMembers.AddUserToGroup(Context, SingleUser, groups.SingleUser, GroupRole.Admin, cancellationToken);
    }
}
