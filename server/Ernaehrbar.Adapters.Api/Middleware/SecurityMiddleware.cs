using System.Security.Claims;
using Ernaehrbar.Adapters.Api.Utilities;
using Ernaehrbar.Parts.Ports;
using Serilog.Context;

namespace Ernaehrbar.Adapters.Api.Middleware;

/// <summary>
/// Security middleware that automatically creates users in the database when they authenticate via Supabase.
/// 
/// This middleware:
/// 1. Extracts user information from JWT token (Supabase)
/// 2. Checks if user exists in local database
/// 3. Creates user automatically if not found (like zentreo pattern)
/// 
/// This ensures that any user who can authenticate via Supabase is automatically
/// available in the local database without manual user creation.
/// </summary>
public class SecurityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityMiddleware> _logger;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IUserRepository _userRepository;
    private readonly SupabaseJwtUtility _supabaseJwtUtility;

    public SecurityMiddleware(
        RequestDelegate next,
        ILogger<SecurityMiddleware> logger,
        IUserReadRepository userReadRepository,
        IUserRepository userRepository,
        SupabaseJwtUtility supabaseJwtUtility)
    {
        _next = next;
        _logger = logger;
        _userReadRepository = userReadRepository;
        _userRepository = userRepository;
        _supabaseJwtUtility = supabaseJwtUtility;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only process authenticated requests
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            await ProcessAuthenticatedUserAsync(context);
        }

        // Continue with the request pipeline
        await _next(context);
    }

    private async Task ProcessAuthenticatedUserAsync(HttpContext context)
    {
        try
        {
            // Extract Supabase user information from JWT token
            var supabaseJwtContent = _supabaseJwtUtility.ToSupabaseJwtContent(context.User);
            
            if (supabaseJwtContent == null)
            {
                _logger.LogWarning("Could not parse Supabase JWT content from authenticated user");
                return;
            }

            // Check if user exists in local database
            var userDto = await _userReadRepository.GetBySupabaseUserIdAsync(
                supabaseJwtContent.UserId,
                context.RequestAborted);

            // If user doesn't exist, create it automatically (like zentreo pattern)
            if (userDto == null)
            {
                _logger.LogWarning(
                    "No user mapping for Supabase user: {SupabaseUserId} - {Email}. Creating user automatically.",
                    supabaseJwtContent.UserId,
                    supabaseJwtContent.Email);

                userDto = await _userRepository.CreateUserAsync(
                    supabaseJwtContent.UserId,
                    supabaseJwtContent.Email,
                    context.RequestAborted);

                _logger.LogInformation(
                    "Created user automatically: {UserId} - {Email}",
                    userDto.Id,
                    userDto.Email);
            }

            // Store user ID in HttpContext.Items for use in controllers
            context.Items["UserId"] = userDto.Id;
            context.Items["UserEmail"] = userDto.Email;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing authenticated user in SecurityMiddleware");
            // Don't throw - let the request continue, but user won't be available
        }
    }
}
