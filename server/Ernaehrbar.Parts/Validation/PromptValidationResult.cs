namespace Ernaehrbar.Parts.Validation;

/// <summary>
/// Ergebnis der Prompt-Validierung für die Rezeptgenerierung.
/// </summary>
public class PromptValidationResult
{
    public bool IsValid { get; init; }
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Gültiges Validierungsergebnis.
    /// </summary>
    public static PromptValidationResult Valid() => new() { IsValid = true };

    /// <summary>
    /// Ungültiges Validierungsergebnis mit Fehlermeldung.
    /// </summary>
    public static PromptValidationResult Invalid(string errorMessage) => new()
    {
        IsValid = false,
        ErrorMessage = errorMessage
    };
}
