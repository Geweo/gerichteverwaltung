using Ernaehrbar.Parts.Models;
using Ernaehrbar.Parts.Ports;
using MediatR;

namespace Ernaehrbar.Parts.Commands;

/// <summary>
/// Command zur Generierung eines Wochenplans mit Rezepten.
/// </summary>
public record GenerateRecipesCommand(
    string Prompt,
    List<MealCategory> MealCategories,
    int NumberOfDays) : IRequest<MealPlanResult>;
