using Ernaehrbar.Parts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ernaehrbar.Adapters.Api.Controllers;

/// <summary>
/// Controller for meal plan operations. Delegiert an MediatR (Queries).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MealPlansController : BaseController
{
    private readonly IMediator _mediator;

    public MealPlansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all meal plans for a group with optional filtering.
    /// </summary>
    /// <param name="groupId">Group ID (required)</param>
    /// <param name="startDateFrom">Optional: Filter by start date (from)</param>
    /// <param name="startDateTo">Optional: Filter by start date (to)</param>
    /// <param name="skip">Optional: Number of items to skip (pagination)</param>
    /// <param name="take">Optional: Number of items to take (pagination)</param>
    [HttpGet]
    public async Task<IActionResult> GetMealPlans(
        [FromQuery] int groupId,
        [FromQuery] DateTime? startDateFrom = null,
        [FromQuery] DateTime? startDateTo = null,
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetMealPlansQuery(groupId, startDateFrom, startDateTo, skip, take);
            var mealPlans = await _mediator.Send(query, cancellationToken);
            return Ok(mealPlans);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get a meal plan by ID.
    /// </summary>
    /// <param name="id">Meal plan ID</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMealPlanById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetMealPlanByIdQuery(id);
            var mealPlan = await _mediator.Send(query, cancellationToken);

            if (mealPlan is null)
            {
                return NotFound(new { error = "Meal plan not found" });
            }

            return Ok(mealPlan);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
