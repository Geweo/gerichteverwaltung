namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Status of a recipe draft during review process.
/// </summary>
public enum DraftStatus
{
    /// <summary>
    /// Pending review.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Approved and converted to Recipe.
    /// </summary>
    Approved = 2,

    /// <summary>
    /// Rejected and not converted.
    /// </summary>
    Rejected = 3
}
