using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Ports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ernaehrbar.Adapters.Api.Controllers;

/// <summary>
/// Controller for recipe draft operations. Delegiert an MediatR (Commands/Handlers).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecipeDraftsController : BaseController
{
    private readonly IMediator _mediator;

    public RecipeDraftsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new recipe draft.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateRecipeDraft(
        [FromBody] CreateRecipeDraftRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var source = Enum.Parse<RecipeSource>(request.Source, ignoreCase: true);
            var mealCategory = request.MealCategory != null
                ? Enum.Parse<MealCategory>(request.MealCategory, ignoreCase: true)
                : (MealCategory?)null;

            var cmd = new CreateRecipeDraftCommand(
                GroupId: request.GroupId,
                CreatedByUserId: request.CreatedByUserId,
                Name: request.Name,
                Source: source,
                Description: request.Description,
                Instructions: request.Instructions,
                MealCategory: mealCategory,
                Ingredients: request.Ingredients?.Select(i => new CreateRecipeDraftIngredientDto(
                    i.Name,
                    i.Quantity,
                    i.Unit,
                    i.Notes
                )).ToList(),
                OriginalData: request.OriginalData
            );

            var result = await _mediator.Send(cmd, cancellationToken);
            return CreatedAtAction(nameof(GetRecipeDraftById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Gets a recipe draft by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipeDraftById(int id, CancellationToken cancellationToken)
    {
        // TODO: Implement Query + Handler
        return NotFound(new { error = "Not implemented yet" });
    }

    /// <summary>
    /// Approves a recipe draft and converts it to a recipe.
    /// </summary>
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveRecipeDraft(
        int id,
        [FromBody] ApproveRecipeDraftRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var cmd = new ApproveRecipeDraftCommand(
                DraftId: id,
                ApprovedByUserId: request.ApprovedByUserId
            );

            var result = await _mediator.Send(cmd, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

/// <summary>
/// Request DTO für CreateRecipeDraft.
/// </summary>
public record CreateRecipeDraftRequest(
    int GroupId,
    int CreatedByUserId,
    string Name,
    string Source, // "Generated", "Upload", "Manual"
    string? Description = null,
    string? Instructions = null,
    string? MealCategory = null, // "Breakfast", "Lunch", "Dinner", "Snack"
    List<CreateRecipeDraftIngredientRequest>? Ingredients = null,
    string? OriginalData = null
);

/// <summary>
/// Request DTO für RecipeDraft-Ingredient.
/// </summary>
public record CreateRecipeDraftIngredientRequest(
    string Name,
    decimal? Quantity = null,
    string? Unit = null,
    string? Notes = null
);

/// <summary>
/// Request DTO für ApproveRecipeDraft.
/// </summary>
public record ApproveRecipeDraftRequest(
    int ApprovedByUserId
);
