using Ernaehrbar.Adapters.Api.Models;
using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Queries;
using Ernaehrbar.Parts.Validation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ernaehrbar.Adapters.Api.Controllers;

/// <summary>
/// Controller for recipe operations. Delegiert an MediatR (Commands/Handlers).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecipesController : BaseController
{
    private readonly IMediator _mediator;

    public RecipesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Generates recipes for a meal plan based on prompt and meal categories.
    /// Validierung erfolgt über FluentValidation (GenerateRecipesCommandValidator).
    /// </summary>
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateRecipes(
        [FromBody] GenerateRecipesRequest request,
        CancellationToken cancellationToken)
    {
        // Validierung erfolgt automatisch über FluentValidation (GenerateRecipesCommandValidator)
        // Prompt-Validierung bleibt als zusätzliche Business-Logik
        var promptValidation = ValidatePrompt(request.Prompt);
        if (!promptValidation.IsValid)
        {
            return BadRequest(new { error = promptValidation.ErrorMessage });
        }

        try
        {
            var cmd = new GenerateRecipesCommand(request.Prompt, request.MealCategories, request.NumberOfDays);
            var result = await _mediator.Send(cmd, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Regenerates a single recipe.
    /// Validierung erfolgt über FluentValidation (RegenerateRecipeCommandValidator).
    /// </summary>
    [HttpPost("regenerate")]
    public async Task<IActionResult> RegenerateRecipe(
        [FromBody] RegenerateRecipeRequest request,
        CancellationToken cancellationToken)
    {
        // Validierung erfolgt automatisch über FluentValidation (RegenerateRecipeCommandValidator)
        try
        {
            var cmd = new RegenerateRecipeCommand(
                request.OriginalPrompt,
                request.NewPrompt,
                request.MealCategory,
                request.ExistingTags ?? new List<string>());
            var recipe = await _mediator.Send(cmd, cancellationToken);
            return Ok(recipe);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Placeholder endpoint for recipe upload. Delegiert an UploadRecipeCommand.
    /// </summary>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadRecipe(CancellationToken cancellationToken)
    {
        var cmd = new UploadRecipeCommand();
        await _mediator.Send(cmd, cancellationToken);
        return Ok(new { message = "Recipe upload endpoint - to be implemented" });
    }

    /// <summary>
    /// Get all recipes for a group with optional filtering.
    /// </summary>
    /// <param name="groupId">Group ID (required)</param>
    /// <param name="tagIds">Optional: Filter by tag IDs (comma-separated)</param>
    /// <param name="searchTerm">Optional: Search in name and description</param>
    /// <param name="skip">Optional: Number of items to skip (pagination)</param>
    /// <param name="take">Optional: Number of items to take (pagination)</param>
    [HttpGet]
    public async Task<IActionResult> GetRecipes(
        [FromQuery] int groupId,
        [FromQuery] List<int>? tagIds = null,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetRecipesQuery(groupId, tagIds, searchTerm, skip, take);
            var recipes = await _mediator.Send(query, cancellationToken);
            return Ok(recipes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get a recipe by ID.
    /// </summary>
    /// <param name="id">Recipe ID</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipeById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetRecipeByIdQuery(id);
            var recipe = await _mediator.Send(query, cancellationToken);

            if (recipe is null)
            {
                return NotFound(new { error = "Recipe not found" });
            }

            return Ok(recipe);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Validates a prompt for recipe generation.
    /// </summary>
    private static PromptValidationResult ValidatePrompt(string prompt)
    {
        return PromptValidator.ValidatePrompt(prompt);
    }
}

