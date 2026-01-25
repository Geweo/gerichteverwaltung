using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;
using Ernaehrbar.Parts.Ports;
using Entities = Ernaehrbar.Adapters.Infrastructure.Data.Entities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for RecipeDrafts.
/// </summary>
public class RecipeDraftFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public RecipeDraft PendingDraft { get; private set; } = null!;
    public RecipeDraft ApprovedDraft { get; private set; } = null!;

    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        var groups = Parent.GroupFixture;
        var users = Parent.UserFixture;

        // Pending draft
        PendingDraft = new RecipeDraft
        {
            GroupId = groups.FamilieMueller.Id,
            Name = "Lasagne",
            Description = "Italienische Lasagne mit Hackfleisch",
            Instructions = "1. Hackfleisch anbraten\n2. Bechamelsauce zubereiten\n3. Schichten\n4. Im Ofen backen",
            Source = Entities.RecipeSource.Generated,
            MealCategory = MealCategory.Dinner,
            CreatedByUserId = users.MaxMueller.Id,
            Status = Entities.DraftStatus.Pending
        };
        await Context.RecipeDrafts.AddAsync(PendingDraft, cancellationToken);

        // Approved draft
        ApprovedDraft = new RecipeDraft
        {
            GroupId = groups.WGBerlin.Id,
            Name = "Pasta Carbonara",
            Description = "Klassische Carbonara",
            Instructions = "1. Nudeln kochen\n2. Speck anbraten\n3. Eier und Parmesan vermischen\n4. Alles vermengen",
            Source = Entities.RecipeSource.Manual,
            MealCategory = MealCategory.Dinner,
            CreatedByUserId = users.TomBerlin.Id,
            ReviewedByUserId = users.LisaBerlin.Id,
            ReviewedAt = DateTime.UtcNow,
            Status = Entities.DraftStatus.Approved
        };
        await Context.RecipeDrafts.AddAsync(ApprovedDraft, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken);

        // Add ingredients to drafts
        var draftIngredients = Parent.RecipeDraftIngredientFixture;
        await draftIngredients.AddIngredient(PendingDraft, "Lasagneplatten", 12, "Stück", 1, cancellationToken);
        await draftIngredients.AddIngredient(PendingDraft, "Hackfleisch", 500, "g", 2, cancellationToken);
        await draftIngredients.AddIngredient(ApprovedDraft, "Spaghetti", 400, "g", 1, cancellationToken);
        await draftIngredients.AddIngredient(ApprovedDraft, "Speck", 200, "g", 2, cancellationToken);
    }
}
