using Ernaehrbar.Parts.Ports;
using Ernaehrbar.Parts.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ernaehrbar.Adapters.Api.Controllers;

/// <summary>
/// Controller for recipe operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecipesController : BaseController
{
    private readonly GeneratePlan _generatePlan;

    public RecipesController(GeneratePlan generatePlan)
    {
        _generatePlan = generatePlan;
    }

    /// <summary>
    /// Generates recipes for a meal plan based on prompt and meal categories.
    /// </summary>
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateRecipes(
        [FromBody] GenerateRecipesRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return BadRequest(new { error = "Prompt is required" });
        }

        // Validate prompt quality
        var promptValidation = ValidatePrompt(request.Prompt);
        if (!promptValidation.IsValid)
        {
            return BadRequest(new { error = promptValidation.ErrorMessage });
        }

        if (request.MealCategories.Count == 0)
        {
            return BadRequest(new { error = "At least one meal category must be selected" });
        }

        if (request.NumberOfDays < 7 || request.NumberOfDays > 21)
        {
            return BadRequest(new { error = "NumberOfDays must be between 7 and 21" });
        }

        try
        {
            var result = await _generatePlan.ExecuteAsync(
                request.Prompt,
                request.MealCategories,
                request.NumberOfDays,
                cancellationToken);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Regenerates a single recipe.
    /// </summary>
    [HttpPost("regenerate")]
    public async Task<IActionResult> RegenerateRecipe(
        [FromBody] RegenerateRecipeRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.OriginalPrompt))
        {
            return BadRequest(new { error = "OriginalPrompt is required" });
        }

        try
        {
            var recipe = await _generatePlan.RegenerateRecipeAsync(
                request.OriginalPrompt,
                request.NewPrompt,
                request.MealCategory,
                request.ExistingTags ?? new List<string>(),
                cancellationToken);

            return Ok(recipe);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Placeholder endpoint for recipe upload.
    /// </summary>
    [HttpPost("upload")]
    public IActionResult UploadRecipe()
    {
        // TODO: Implement recipe upload
        return Ok(new { message = "Recipe upload endpoint - to be implemented" });
    }

    /// <summary>
    /// Get all recipes for the current user.
    /// </summary>
    [HttpGet]
    public IActionResult GetRecipes()
    {
        // TODO: Implement recipe listing
        return Ok(new { recipes = Array.Empty<object>() });
    }

    /// <summary>
    /// Validates a prompt for recipe generation.
    /// </summary>
    private static PromptValidationResult ValidatePrompt(string prompt)
    {
        return PromptValidator.ValidatePrompt(prompt);
    }
}

/// <summary>
/// Request model for recipe generation.
/// </summary>
public class GenerateRecipesRequest
{
    public required string Prompt { get; init; }
    public required List<MealCategory> MealCategories { get; init; }
    public int NumberOfDays { get; init; } = 7;
}

/// <summary>
/// Request model for recipe regeneration.
/// </summary>
public class RegenerateRecipeRequest
{
    public required string OriginalPrompt { get; init; }
    public string? NewPrompt { get; init; }
    public required MealCategory MealCategory { get; init; }
    public List<string>? ExistingTags { get; init; }
}

/// <summary>
/// Result of prompt validation.
/// </summary>
internal class PromptValidationResult
{
    public bool IsValid { get; init; }
    public string? ErrorMessage { get; init; }

    public static PromptValidationResult Valid() => new() { IsValid = true };
    public static PromptValidationResult Invalid(string errorMessage) => new() { IsValid = false, ErrorMessage = errorMessage };
}

/// <summary>
/// Validates that a prompt is meaningful for recipe generation.
/// </summary>
internal static class PromptValidator
{
    private static readonly HashSet<string> RecipeRelatedKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "rezept", "rezepte", "gericht", "gerichte", "essen", "mahlzeit", "mahlzeiten",
        "kochen", "küche", "zutaten", "speise", "speisen", "mahl", "frühstück",
        "mittagessen", "abendessen", "snack", "vegetarisch", "vegan", "fleisch",
        "fisch", "gesund", "schnell", "einfach", "italienisch", "asiatisch",
        "mediterran", "deutsch", "frisch", "warm", "kalt", "salat", "suppe",
        "pasta", "pizza", "curry", "stir", "fry", "bake", "grill", "salmon",
        "chicken", "beef", "pork", "vegetables", "vegetables", "fruit", "salad"
    };

    public static PromptValidationResult ValidatePrompt(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return PromptValidationResult.Invalid("Der Prompt darf nicht leer sein.");
        }

        // Minimum length check
        var trimmedPrompt = prompt.Trim();
        if (trimmedPrompt.Length < 3)
        {
            return PromptValidationResult.Invalid("Der Prompt muss mindestens 3 Zeichen lang sein.");
        }

        // Check if prompt contains meaningful words (at least 3 characters)
        var words = trimmedPrompt.Split(new[] { ' ', ',', '.', '!', '?', '\n', '\r', '\t' }, 
            StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length >= 3)
            .ToList();

        if (words.Count == 0)
        {
            return PromptValidationResult.Invalid("Der Prompt muss mindestens ein sinnvolles Wort enthalten.");
        }

        // Check if prompt seems related to food/recipes
        // If it's very short (less than 10 chars), it's likely not meaningful
        if (trimmedPrompt.Length < 10)
        {
            // For very short prompts, check if they contain recipe-related keywords
            var hasRecipeKeyword = words.Any(w => RecipeRelatedKeywords.Contains(w));
            if (!hasRecipeKeyword)
            {
                return PromptValidationResult.Invalid(
                    "Bitte beschreibe, welche Art von Rezepten oder Gerichten du möchtest. " +
                    "Beispiele: 'Gesunde vegetarische Rezepte', 'Schnelle Gerichte für die Woche', 'Italienische Küche'");
            }
        }

        // Check for obvious non-food related content
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
