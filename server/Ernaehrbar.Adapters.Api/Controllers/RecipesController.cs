using Microsoft.AspNetCore.Mvc;

namespace Ernaehrbar.Adapters.Api.Controllers;

/// <summary>
/// Controller for recipe operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RecipesController : BaseController
{
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

