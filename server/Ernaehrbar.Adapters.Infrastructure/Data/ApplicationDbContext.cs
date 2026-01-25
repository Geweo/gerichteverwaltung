using System.Reflection;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
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

    // Groups & Users
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<GroupMember> GroupMembers { get; set; } = null!;
    public DbSet<GroupInvite> GroupInvites { get; set; } = null!;

    // Recipes
    public DbSet<Recipe> Recipes { get; set; } = null!;
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<RecipeTag> RecipeTags { get; set; } = null!;
    public DbSet<RecipeRating> RecipeRatings { get; set; } = null!;
    public DbSet<NutritionInfo> NutritionInfos { get; set; } = null!;

    // Meal Plans
    public DbSet<MealPlan> MealPlans { get; set; } = null!;
    public DbSet<MealPlanEntry> MealPlanEntries { get; set; } = null!;

    // Recipe Drafts
    public DbSet<RecipeDraft> RecipeDrafts { get; set; } = null!;
    public DbSet<RecipeDraftIngredient> RecipeDraftIngredients { get; set; } = null!;

    // Notifications
    public DbSet<Notification> Notifications { get; set; } = null!;

    // Upload Tasks
    public DbSet<UploadTask> UploadTasks { get; set; } = null!;

    // Files
    public DbSet<Entities.File> Files { get; set; } = null!;

    // Shopping Lists
    public DbSet<ShoppingList> ShoppingLists { get; set; } = null!;
    public DbSet<ShoppingListItem> ShoppingListItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Load all IEntityTypeConfiguration (Entity Maps) automatically
        // This replaces the manual Configure* methods below
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // All entity configurations are now in Map classes (IEntityTypeConfiguration)
        // They are automatically loaded via ApplyConfigurationsFromAssembly above
    }

    /// <summary>
    /// The states we want to observe for automatic timestamp updates.
    /// </summary>
    private readonly HashSet<EntityState> _managedStates =
        [EntityState.Added, EntityState.Modified, EntityState.Deleted];

    /// <summary>
    /// Automatically sets CreatedAt and UpdatedAt timestamps for BaseEntity instances.
    /// </summary>
    private void AddTimestamps()
    {
        var transactionTime = DateTime.UtcNow;

        ChangeTracker.Entries()
            .Where(x => _managedStates.Contains(x.State))
            .Where(x => x.Entity is BaseEntity)
            .ToList()
            .ForEach(x =>
            {
                // Type guard
                if (x.Entity is not BaseEntity entity) return;

                // Update "UpdatedAt" for all managed states
                entity.UpdatedAt = transactionTime;

                // If the entity is newly added and does not have a CreatedAt value
                // => set it to the current time
                if (x.State == EntityState.Added && entity.CreatedAt == default)
                {
                    entity.CreatedAt = transactionTime;
                }
            });
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        AddTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

}

