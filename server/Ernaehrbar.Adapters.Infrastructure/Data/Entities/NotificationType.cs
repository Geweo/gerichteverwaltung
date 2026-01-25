namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Type of notification.
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// Upload processing completed.
    /// </summary>
    UploadComplete = 1,

    /// <summary>
    /// Recipe generation completed.
    /// </summary>
    GenerationComplete = 2,

    /// <summary>
    /// Meal plan is ready.
    /// </summary>
    MealPlanReady = 3,

    /// <summary>
    /// Warning about similar recipe found.
    /// </summary>
    SimilarRecipeWarning = 4
}
