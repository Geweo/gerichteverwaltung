using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Development fixture set that seeds all tables with test data.
/// </summary>
public class DevelopmentFixtureSet : SeedableFixture
{
    public GroupFixture GroupFixture { get; } = new();
    public UserFixture UserFixture { get; } = new();
    public GroupMemberFixture GroupMemberFixture { get; } = new();
    public GroupInviteFixture GroupInviteFixture { get; } = new();
    public TagFixture TagFixture { get; } = new();
    public RecipeFixture RecipeFixture { get; } = new();
    public RecipeIngredientFixture RecipeIngredientFixture { get; } = new();
    public RecipeTagFixture RecipeTagFixture { get; } = new();
    public RecipeRatingFixture RecipeRatingFixture { get; } = new();
    public NutritionInfoFixture NutritionInfoFixture { get; } = new();
    public MealPlanFixture MealPlanFixture { get; } = new();
    public MealPlanEntryFixture MealPlanEntryFixture { get; } = new();
    public RecipeDraftFixture RecipeDraftFixture { get; } = new();
    public RecipeDraftIngredientFixture RecipeDraftIngredientFixture { get; } = new();
    public NotificationFixture NotificationFixture { get; } = new();
    public UploadTaskFixture UploadTaskFixture { get; } = new();
    public FileFixture FileFixture { get; } = new();
    public ShoppingListFixture ShoppingListFixture { get; } = new();
    public ShoppingListItemFixture ShoppingListItemFixture { get; } = new();

    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        // 1. Groups first (no dependencies)
        await GroupFixture.Seed(Context, this, cancellationToken);

        // 2. Users (no dependencies)
        await UserFixture.Seed(Context, this, cancellationToken);

        // 3. GroupMembers (depends on Groups and Users)
        await GroupMemberFixture.Seed(Context, this, cancellationToken);

        // 4. GroupInvites (depends on Groups and Users)
        await GroupInviteFixture.Seed(Context, this, cancellationToken);

        // 5. Tags (depends on Groups)
        await TagFixture.Seed(Context, this, cancellationToken);

        // 6. Recipes (depends on Groups)
        await RecipeFixture.Seed(Context, this, cancellationToken);

        // 7. RecipeIngredients, RecipeTags, NutritionInfo (depends on Recipes)
        // These are created by RecipeFixture, but we seed them explicitly for clarity
        await RecipeIngredientFixture.Seed(Context, this, cancellationToken);
        await RecipeTagFixture.Seed(Context, this, cancellationToken);
        await NutritionInfoFixture.Seed(Context, this, cancellationToken);

        // 8. RecipeRatings (depends on Recipes and Users)
        await RecipeRatingFixture.Seed(Context, this, cancellationToken);

        // 9. MealPlans (depends on Groups)
        await MealPlanFixture.Seed(Context, this, cancellationToken);

        // 10. MealPlanEntries (depends on MealPlans and Recipes)
        await MealPlanEntryFixture.Seed(Context, this, cancellationToken);

        // 11. RecipeDrafts (depends on Groups and Users)
        await RecipeDraftFixture.Seed(Context, this, cancellationToken);

        // 12. RecipeDraftIngredients (depends on RecipeDrafts)
        await RecipeDraftIngredientFixture.Seed(Context, this, cancellationToken);

        // 13. Notifications (depends on Users)
        await NotificationFixture.Seed(Context, this, cancellationToken);

        // 14. UploadTasks (depends on Users, Groups, and RecipeDrafts)
        await UploadTaskFixture.Seed(Context, this, cancellationToken);

        // 15. Files (depends on Groups, Users, Recipes, and RecipeDrafts)
        await FileFixture.Seed(Context, this, cancellationToken);

        // 16. ShoppingLists (depends on Groups and Users)
        await ShoppingListFixture.Seed(Context, this, cancellationToken);

        // 17. ShoppingListItems (depends on ShoppingLists and RecipeIngredients)
        await ShoppingListItemFixture.Seed(Context, this, cancellationToken);
    }
}
