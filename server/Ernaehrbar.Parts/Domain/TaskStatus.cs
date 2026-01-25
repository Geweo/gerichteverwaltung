namespace Ernaehrbar.Parts.Domain;

/// <summary>
/// Status of an upload/background task.
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// Task is pending processing.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Task is currently being processed.
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Task completed successfully.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Task failed with error.
    /// </summary>
    Failed = 4
}
