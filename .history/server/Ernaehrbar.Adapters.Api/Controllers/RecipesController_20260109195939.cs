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
