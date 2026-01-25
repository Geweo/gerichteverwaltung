using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for UploadTasks.
/// </summary>
public class UploadTaskFixture : SeedableFixture<DevelopmentFixtureSet>
{
    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        var groups = Parent.GroupFixture;
        var users = Parent.UserFixture;
        var drafts = Parent.RecipeDraftFixture;

        var task1 = new UploadTask
        {
            UserId = users.MaxMueller.Id,
            GroupId = groups.FamilieMueller.Id,
            FileName = "rezept.pdf",
            FilePath = "uploads/rezept.pdf",
            Status = Ernaehrbar.Adapters.Infrastructure.Data.Entities.TaskStatus.Completed,
            RecipeDraftId = drafts.PendingDraft.Id
        };
        await Context.UploadTasks.AddAsync(task1, cancellationToken);

        var task2 = new UploadTask
        {
            UserId = users.TomBerlin.Id,
            GroupId = groups.WGBerlin.Id,
            FileName = "kochbuch-seite.jpg",
            FilePath = "uploads/kochbuch-seite.jpg",
            Status = Ernaehrbar.Adapters.Infrastructure.Data.Entities.TaskStatus.Processing
        };
        await Context.UploadTasks.AddAsync(task2, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken);
    }
}
