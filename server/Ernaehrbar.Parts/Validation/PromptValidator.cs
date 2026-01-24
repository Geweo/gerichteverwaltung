namespace Ernaehrbar.Parts.Validation;

/// <summary>
/// Validiert, ob ein Prompt für die Rezeptgenerierung sinnvoll ist.
/// Domänenlogik: Was macht einen gültigen Rezept-Prompt aus?
/// </summary>
public static class PromptValidator
{
    private static readonly HashSet<string> RecipeRelatedKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "rezept", "rezepte", "gericht", "gerichte", "essen", "mahlzeit", "mahlzeiten",
        "kochen", "küche", "zutaten", "speise", "speisen", "mahl", "frühstück",
        "mittagessen", "abendessen", "snack", "vegetarisch", "vegan", "fleisch",
        "fisch", "gesund", "schnell", "einfach", "italienisch", "asiatisch",
        "mediterran", "deutsch", "frisch", "warm", "kalt", "salat", "suppe",
        "pasta", "pizza", "curry", "stir", "fry", "bake", "grill", "salmon",
        "chicken", "beef", "pork", "vegetables", "fruit", "salad"
    };

    /// <summary>
    /// Prüft, ob der Prompt für die Rezeptgenerierung geeignet ist.
    /// </summary>
    /// <param name="prompt">Der zu validierende Prompt.</param>
    /// <returns>Validierungsergebnis mit IsValid und ggf. ErrorMessage.</returns>
    public static PromptValidationResult ValidatePrompt(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return PromptValidationResult.Invalid("Der Prompt darf nicht leer sein.");
        }

        var trimmedPrompt = prompt.Trim();
        if (trimmedPrompt.Length < 3)
        {
            return PromptValidationResult.Invalid("Der Prompt muss mindestens 3 Zeichen lang sein.");
        }

        var words = trimmedPrompt
            .Split(new[] { ' ', ',', '.', '!', '?', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length >= 3)
            .ToList();

        if (words.Count == 0)
        {
            return PromptValidationResult.Invalid("Der Prompt muss mindestens ein sinnvolles Wort enthalten.");
        }

        if (trimmedPrompt.Length < 10)
        {
            var hasRecipeKeyword = words.Any(w => RecipeRelatedKeywords.Contains(w));
            if (!hasRecipeKeyword)
            {
                return PromptValidationResult.Invalid(
                    "Bitte beschreibe, welche Art von Rezepten oder Gerichten du möchtest. " +
                    "Beispiele: 'Gesunde vegetarische Rezepte', 'Schnelle Gerichte für die Woche', 'Italienische Küche'");
            }
        }

        var lowerPrompt = trimmedPrompt.ToLowerInvariant();
        var nonFoodIndicators = new[] { "test", "tesa", "abc", "123", "xyz", "asdf", "qwerty" };
        if (nonFoodIndicators.Contains(lowerPrompt))
        {
            return PromptValidationResult.Invalid(
                "Bitte beschreibe, welche Art von Rezepten oder Gerichten du möchtest. " +
                "Beispiele: 'Gesunde vegetarische Rezepte', 'Schnelle Gerichte für die Woche'");
        }

        return PromptValidationResult.Valid();
    }
}
