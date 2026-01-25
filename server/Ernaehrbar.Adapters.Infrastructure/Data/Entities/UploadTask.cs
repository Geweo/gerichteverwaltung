namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Represents an upload task for tracking file processing status.
/// </summary>
public class UploadTask : BaseEntity
{
    /// <summary>
    /// Foreign key to the user who uploaded the file.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to the user.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Foreign key to the group this upload belongs to.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Navigation property to the group.
    /// </summary>
    public Group Group { get; set; } = null!;

    /// <summary>
    /// Original filename.
    /// </summary>
    public required string FileName { get; set; }

    /// <summary>
    /// Path to the file in storage (S3/Supabase).
    /// </summary>
    public required string FilePath { get; set; }

    /// <summary>
    /// Status of the upload task.
    /// </summary>
    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    /// <summary>
    /// Error message if the task failed.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Foreign key to the recipe draft created from this upload (nullable).
    /// </summary>
    public int? RecipeDraftId { get; set; }

    /// <summary>
    /// Navigation property to the recipe draft.
    /// </summary>
    public RecipeDraft? RecipeDraft { get; set; }
}
