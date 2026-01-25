using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for UploadTask entity.
/// </summary>
public class UploadTaskEntityMap : BaseMap<UploadTask>
{
    public override void Configure(EntityTypeBuilder<UploadTask> entity)
    {
        base.Configure(entity);

        entity.ToTable("UploadTasks");

        entity.Property(e => e.FileName)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(e => e.FilePath)
            .IsRequired()
            .HasMaxLength(1000);

        entity.Property(e => e.Error)
            .HasMaxLength(2000);

        // User relationship
        entity.HasOne(ut => ut.User)
            .WithMany(u => u.UploadTasks)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Group relationship
        entity.HasOne(ut => ut.Group)
            .WithMany(g => g.UploadTasks)
            .HasForeignKey(ut => ut.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // RecipeDraft relationship (nullable)
        entity.HasOne(ut => ut.RecipeDraft)
            .WithMany()
            .HasForeignKey(ut => ut.RecipeDraftId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
