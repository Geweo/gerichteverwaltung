using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;

namespace Ernaehrbar.Tests.Integration.TestHelpers;

/// <summary>
/// Helper-Klasse zum Erstellen von Test-Daten.
/// </summary>
public class TestFixtures
{
    private readonly ApplicationDbContext _context;

    public TestFixtures(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUser(string supabaseUserId, string email, string? displayName = null)
    {
        var user = new User
        {
            SupabaseUserId = supabaseUserId,
            Email = email,
            DisplayName = displayName
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<Group> CreateGroup(string name, string? description = null)
    {
        var group = new Group
        {
            Name = name,
            Description = description
        };
        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();
        return group;
    }

    public async Task<GroupMember> AddUserToGroup(User user, Group group, GroupRole role = GroupRole.Member)
    {
        var member = new GroupMember
        {
            UserId = user.Id,
            GroupId = group.Id,
            Role = role,
            JoinedAt = DateTime.UtcNow
        };
        await _context.GroupMembers.AddAsync(member);
        await _context.SaveChangesAsync();
        return member;
    }
}
