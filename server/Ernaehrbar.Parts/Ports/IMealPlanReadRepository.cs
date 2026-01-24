using Ernaehrbar.Parts.ReadModels;

namespace Ernaehrbar.Parts.Ports;

/// <summary>
/// Port für Read-optimierte Wochenplan-Abfragen.
/// </summary>
public interface IMealPlanReadRepository
{
    /// <summary>
    /// Ruft einen Wochenplan anhand der ID ab.
    /// </summary>
    Task<MealPlanReadModel?> GetByIdAsync(int mealPlanId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ruft eine Liste von Wochenplänen für eine Gruppe ab.
    /// </summary>
    Task<List<MealPlanReadModel>> GetMealPlansAsync(
        int groupId,
        DateTime? startDateFrom = null,
        DateTime? startDateTo = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default);
}
