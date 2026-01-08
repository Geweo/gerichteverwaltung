using Microsoft.AspNetCore.Mvc;

namespace Ernaehrbar.Adapters.Api.Controllers;

/// <summary>
/// Base controller for API endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
}

