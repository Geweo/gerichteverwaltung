namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Type of uploaded file.
/// </summary>
public enum FileType
{
    /// <summary>
    /// Image file (PNG, JPG, etc.).
    /// </summary>
    Image = 1,

    /// <summary>
    /// PDF file.
    /// </summary>
    Pdf = 2,

    /// <summary>
    /// Other file type.
    /// </summary>
    Other = 3
}
