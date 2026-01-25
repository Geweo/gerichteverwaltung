using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;
using Entities = Ernaehrbar.Adapters.Infrastructure.Data.Entities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for Files.
/// </summary>
public class FileFixture : SeedableFixture<DevelopmentFixtureSet>
{
    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        var groups = Parent.GroupFixture;
        var users = Parent.UserFixture;
        var recipes = Parent.RecipeFixture;
        var drafts = Parent.RecipeDraftFixture;

        var file1 = new Entities.File
        {
            GroupId = groups.FamilieMueller.Id,
            UploadedByUserId = users.MaxMueller.Id,
            FileName = "spaghetti-bolognese.jpg",
            FilePath = "files/spaghetti-bolognese.jpg",
            ContentType = "image/jpeg",
            FileSizeBytes = 245760,
            Type = FileType.Image,
            RecipeId = recipes.SpaghettiBolognese.Id
        };
        await Context.Files.AddAsync(file1, cancellationToken);

        var file2 = new Entities.File
        {
            GroupId = groups.FamilieMueller.Id,
            UploadedByUserId = users.MaxMueller.Id,
            FileName = "lasagne-rezept.pdf",
            FilePath = "files/lasagne-rezept.pdf",
            ContentType = "application/pdf",
            FileSizeBytes = 512000,
            Type = FileType.Pdf,
            RecipeDraftId = drafts.PendingDraft.Id
        };
        await Context.Files.AddAsync(file2, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken);
    }
}
