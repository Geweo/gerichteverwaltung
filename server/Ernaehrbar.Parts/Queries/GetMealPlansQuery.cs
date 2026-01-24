using Ernaehrbar.Parts.ReadModels;
using MediatR;

namespace Ernaehrbar.Parts.Queries;

/// <summary>
/// Query zum Abrufen einer Liste von Wochenplänen für eine Gruppe.
/// </summary>
public record GetMealPlansQuery(
    int GroupId,
    DateTime? StartDateFrom = null,
    DateTime? StartDateTo = null,
    int? Skip = null,
    int? Take = null) : IRequest<List<MealPlanReadModel>>;
