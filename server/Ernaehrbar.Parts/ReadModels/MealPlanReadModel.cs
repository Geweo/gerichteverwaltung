using Ernaehrbar.Parts.Ports;

namespace Ernaehrbar.Parts.ReadModels;

/// <summary>
/// Read-optimiertes Model für Wochenpläne (flach, ohne Navigation Properties).
/// Wird für Queries verwendet.
/// </summary>
public record MealPlanReadModel(
    int Id,
    int GroupId,
    DateTime StartDate,
    DateTime EndDate,
    string? Name,
    string? GenerationPrompt,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<MealPlanEntryReadModel> Entries);

/// <summary>
/// Read-optimiertes Model für Wochenplan-Einträge.
/// </summary>
public record MealPlanEntryReadModel(
    int Id,
    int MealPlanId,
    int RecipeId,
    DateTime Date,
    MealCategory MealCategory,
    int DayNumber,
    RecipeReadModel? Recipe);

