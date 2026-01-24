using Ernaehrbar.Parts.ReadModels;
using MediatR;

namespace Ernaehrbar.Parts.Queries;

/// <summary>
/// Query zum Abrufen eines Wochenplans anhand der ID.
/// </summary>
public record GetMealPlanByIdQuery(int MealPlanId) : IRequest<MealPlanReadModel?>;
