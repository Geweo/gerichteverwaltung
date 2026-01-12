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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure base entities
        ConfigureBaseEntities(modelBuilder);

        // Configure groups & users
        ConfigureGroupEntities(modelBuilder);

        // Configure recipes
        ConfigureRecipeEntities(modelBuilder);

        // Configure meal plans
        ConfigureMealPlanEntities(modelBuilder);
    }

    private static void ConfigureBaseEntities(ModelBuilder modelBuilder)
    {
        // BaseEntity: Set CreatedAt and UpdatedAt defaults
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreatedAt))
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.UpdatedAt))
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            }
        }
    }

    private static void ConfigureGroupEntities(ModelBuilder modelBuilder)
    {
        // User: Unique constraint on SupabaseUserId
        modelBuilder.Entity<User>()
            .HasIndex(u => u.SupabaseUserId)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email);

        // GroupMember: Composite unique constraint (User + Group)
        modelBuilder.Entity<GroupMember>()
            .HasIndex(gm => new { gm.UserId, gm.GroupId })
            .IsUnique();

        modelBuilder.Entity<GroupMember>()
            .HasOne(gm => gm.User)
            .WithMany(u => u.GroupMemberships)
            .HasForeignKey(gm => gm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupMember>()
            .HasOne(gm => gm.Group)
            .WithMany(g => g.Members)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // GroupInvite: Unique constraint on Token
        modelBuilder.Entity<GroupInvite>()
            .HasIndex(gi => gi.Token)
            .IsUnique();

        modelBuilder.Entity<GroupInvite>()
            .HasOne(gi => gi.Group)
            .WithMany(g => g.Invites)
            .HasForeignKey(gi => gi.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupInvite>()
            .HasOne(gi => gi.CreatedByUser)
            .WithMany()
            .HasForeignKey(gi => gi.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureRecipeEntities(ModelBuilder modelBuilder)
    {
        // Recipe: Group relationship
        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.Group)
            .WithMany(g => g.Recipes)
            .HasForeignKey(r => r.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // RecipeIngredient
        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Tag: Group relationship
        modelBuilder.Entity<Tag>()
            .HasOne(t => t.Group)
            .WithMany()
            .HasForeignKey(t => t.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Tag: Unique constraint on Name + GroupId
        modelBuilder.Entity<Tag>()
            .HasIndex(t => new { t.Name, t.GroupId })
            .IsUnique();

        // RecipeTag: Composite unique constraint
        modelBuilder.Entity<RecipeTag>()
            .HasIndex(rt => new { rt.RecipeId, rt.TagId })
            .IsUnique();

        modelBuilder.Entity<RecipeTag>()
            .HasOne(rt => rt.Recipe)
            .WithMany(r => r.RecipeTags)
            .HasForeignKey(rt => rt.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeTag>()
            .HasOne(rt => rt.Tag)
            .WithMany(t => t.RecipeTags)
            .HasForeignKey(rt => rt.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        // RecipeRating: Composite unique constraint (User + Recipe)
        modelBuilder.Entity<RecipeRating>()
            .HasIndex(rr => new { rr.UserId, rr.RecipeId })
            .IsUnique();

        modelBuilder.Entity<RecipeRating>()
            .HasOne(rr => rr.Recipe)
            .WithMany(r => r.Ratings)
            .HasForeignKey(rr => rr.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeRating>()
            .HasOne(rr => rr.User)
            .WithMany()
            .HasForeignKey(rr => rr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // NutritionInfo: One-to-one with Recipe
        modelBuilder.Entity<NutritionInfo>()
            .HasOne(ni => ni.Recipe)
            .WithOne(r => r.NutritionInfo)
            .HasForeignKey<NutritionInfo>(ni => ni.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureMealPlanEntities(ModelBuilder modelBuilder)
    {
        // MealPlan: Group relationship
        modelBuilder.Entity<MealPlan>()
            .HasOne(mp => mp.Group)
            .WithMany(g => g.MealPlans)
            .HasForeignKey(mp => mp.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // MealPlanEntry
        modelBuilder.Entity<MealPlanEntry>()
            .HasOne(mpe => mpe.MealPlan)
            .WithMany(mp => mp.Entries)
            .HasForeignKey(mpe => mpe.MealPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MealPlanEntry>()
            .HasOne(mpe => mpe.Recipe)
            .WithMany(r => r.MealPlanEntries)
            .HasForeignKey(mpe => mpe.RecipeId)
            .OnDelete(DeleteBehavior.SetNull);

        // MealPlanEntry: Index on Date and MealCategory for faster queries
        modelBuilder.Entity<MealPlanEntry>()
            .HasIndex(mpe => new { mpe.MealPlanId, mpe.Date, mpe.MealCategory });
    }
}

