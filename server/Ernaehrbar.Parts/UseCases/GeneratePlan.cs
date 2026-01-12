using Ernaehrbar.Parts.Ports;

namespace Ernaehrbar.Parts.UseCases;

/// <summary>
/// Use case for generating a weekly meal plan with recipes.
/// </summary>
public class GeneratePlan
{
    private readonly ILLMService _llmService;

    public GeneratePlan(ILLMService llmService)
    {
        _llmService = llmService;
    }

    /// <summary>
    /// Generates a meal plan with recipes based on prompt and meal categories.
    /// </summary>
    /// <param name="prompt">User prompt describing desired recipes</param>
    /// <param name="mealCategories">Selected meal categories (breakfast, lunch, dinner)</param>
    /// <param name="numberOfDays">Number of days (7-21)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Generated meal plan with recipes</returns>
    public async Task<MealPlanResult> ExecuteAsync(
        string prompt,
        List<MealCategory> mealCategories,
        int numberOfDays,
        CancellationToken cancellationToken = default)
    {
        if (numberOfDays < 7 || numberOfDays > 21)
        {
            throw new ArgumentException("Number of days must be between 7 and 21", nameof(numberOfDays));
        }

        if (mealCategories.Count == 0)
        {
            throw new ArgumentException("At least one meal category must be selected", nameof(mealCategories));
        }

        // Extract tags from prompt
        var tags = await _llmService.ExtractTagsFromPromptAsync(prompt, cancellationToken);

        // Generate recipes
        var recipes = await _llmService.GenerateRecipesAsync(
            prompt,
            mealCategories,
            numberOfDays,
            cancellationToken);

        return new MealPlanResult
        {
            Prompt = prompt,
            Tags = tags,
            Recipes = recipes,
            MealCategories = mealCategories,
            NumberOfDays = numberOfDays,
            GeneratedAt = DateTime.UtcNow,
        };
    }

    /// <summary>
    /// Regenerates a single recipe in the meal plan.
    /// </summary>
    public async Task<GeneratedRecipe> RegenerateRecipeAsync(
        string originalPrompt,
        string? newPrompt,
        MealCategory mealCategory,
        List<string> existingTags,
        CancellationToken cancellationToken = default)
    {
        return await _llmService.RegenerateRecipeAsync(
            originalPrompt,
            newPrompt,
            mealCategory,
            existingTags,
            cancellationToken);
    }
}

/// <summary>
/// Result of meal plan generation.
/// </summary>
public class MealPlanResult
{
    public required string Prompt { get; init; }
    public required List<string> Tags { get; init; }
    public required List<GeneratedRecipe> Recipes { get; init; }
    public required List<MealCategory> MealCategories { get; init; }
    public int NumberOfDays { get; init; }
    public DateTime GeneratedAt { get; init; }
}

