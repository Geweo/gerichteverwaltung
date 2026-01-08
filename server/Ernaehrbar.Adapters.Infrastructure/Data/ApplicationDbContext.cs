using Microsoft.EntityFrameworkCore;

namespace Ernaehrbar.Adapters.Infrastructure.Data;

/// <summary>
/// Application database context for Entity Framework Core.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // TODO: Add DbSets for entities
}

