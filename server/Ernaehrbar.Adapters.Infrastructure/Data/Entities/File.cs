namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents an uploaded file (PDF, image, etc.).
/// </summary>
public class File : BaseGroupEntity
{
    /// <summary>
    /// Foreign key to the user who uploaded the file.
    /// </summary>
    public int UploadedByUserId { get; set; }

    /// <summary>
    /// Navigation property to the user who uploaded the file.
    /// </summary>
    public User UploadedByUser { get; set; } = null!;

    /// <summary>
    /// Original filename.
    /// </summary>
    public required string FileName { get; set; }

    /// <summary>
    /// Path to the file in storage (S3/Supabase).
    /// </summary>
    public required string FilePath { get; set; }

    /// <summary>
    /// MIME content type (e.g., "image/png", "application/pdf").
    /// </summary>
    public required string ContentType { get; set; }

    /// <summary>
    /// File size in bytes.
    /// </summary>
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// Type of file (Image, Pdf, Other).
    /// </summary>
    public FileType Type { get; set; }

    /// <summary>
    /// Foreign key to the recipe this file belongs to (nullable).
    /// </summary>
    public int? RecipeId { get; set; }

    /// <summary>
    /// Navigation property to the recipe.
    /// </summary>
    public Recipe? Recipe { get; set; }

    /// <summary>
    /// Foreign key to the recipe draft this file belongs to (nullable).
    /// </summary>
    public int? RecipeDraftId { get; set; }

    /// <summary>
    /// Navigation property to the recipe draft.
    /// </summary>
    public RecipeDraft? RecipeDraft { get; set; }
}
