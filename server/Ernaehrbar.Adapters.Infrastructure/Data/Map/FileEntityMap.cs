using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FileEntity = Ernaehrbar.Adapters.Infrastructure.Data.Entities.File;

namespace Ernaehrbar.Adapters.Infrastructure.Data.Map;

/// <summary>
/// Entity configuration for File entity.
/// </summary>
public class FileEntityMap : BaseGroupMap<FileEntity>
{
    public override void Configure(EntityTypeBuilder<FileEntity> entity)
    {
        base.Configure(entity);

        entity.ToTable("Files");

        entity.Property(e => e.FileName)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(e => e.FilePath)
            .IsRequired()
            .HasMaxLength(1000);

        entity.Property(e => e.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        // Group relationship
        entity.HasOne(f => f.Group)
            .WithMany(g => g.Files)
            .HasForeignKey(f => f.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // UploadedByUser relationship
        entity.HasOne(f => f.UploadedByUser)
            .WithMany(u => u.UploadedFiles)
            .HasForeignKey(f => f.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Recipe relationship (nullable)
        entity.HasOne(f => f.Recipe)
            .WithMany(r => r.Files)
            .HasForeignKey(f => f.RecipeId)
            .OnDelete(DeleteBehavior.SetNull);

        // RecipeDraft relationship (nullable)
        entity.HasOne(f => f.RecipeDraft)
            .WithMany(rd => rd.Files)
            .HasForeignKey(f => f.RecipeDraftId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
