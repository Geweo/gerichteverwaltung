namespace Ernaehrbar.Adapters.Infrastructure.Data.Entities;

/// <summary>
/// Status of a meal plan.
/// </summary>
public enum MealPlanStatus
{
    /// <summary>
    /// Draft - not yet active.
    /// </summary>
    Draft = 1,

    /// <summary>
    /// Active - currently in use.
    /// </summary>
    Active = 2,

    /// <summary>
    /// Archived - past meal plan.
    /// </summary>
    Archived = 3
}
