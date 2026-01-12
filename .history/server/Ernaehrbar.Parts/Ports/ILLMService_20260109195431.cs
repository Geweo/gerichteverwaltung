namespace Ernaehrbar.Parts.Ports;

/// <summary>
/// Port for LLM (Large Language Model) operations.
/// Used for recipe generation and tag extraction.
/// </summary>
public interface ILLMService
{
    /// <summary>
    /// Generates recipes based on a prompt and meal categories.
    /// </summary>
    /// <param name="prompt">User prompt describing desired recipes</param>
    /// <param name="mealCategories">Selected meal categories (breakfast, lunch, dinner)</param>
    /// <param name="numberOfDays">Number of days to generate recipes for (7-21)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Generated recipes with tags</returns>
    Task<List<GeneratedRecipe>> GenerateRecipesAsync(
        string prompt,
        List<MealCategory> mealCategories,
        int numberOfDays,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts tags from a prompt (e.g., "vegetarisch", "vegan", "frisch").
    /// </summary>
    /// <param name="prompt">User prompt</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of extracted tags</returns>
    Task<List<string>> ExtractTagsFromPromptAsync(
        string prompt,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Regenerates a single recipe with optional new prompt.
    /// </summary>
    /// <param name="originalPrompt">Original prompt used</param>
    /// <param name="newPrompt">Optional new prompt, if null uses original</param>
    /// <param name="mealCategory">Meal category for the recipe</param>
    /// <param name="existingTags">Existing tags to consider</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Regenerated recipe</returns>
    Task<GeneratedRecipe> RegenerateRecipeAsync(
        string originalPrompt,
        string? newPrompt,
        MealCategory mealCategory,
        List<string> existingTags,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Meal categories for recipe generation.
/// </summary>
public enum MealCategory
{
    Breakfast = 1,
    Lunch = 2,
    Dinner = 3,
}

/// <summary>
/// Generated recipe with tags and metadata.
/// </summary>
public class GeneratedRecipe
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Ingredients { get; init; }
    public required List<string> Tags { get; init; }
    public required MealCategory MealCategory { get; init; }
    public int DayNumber { get; init; }
}
