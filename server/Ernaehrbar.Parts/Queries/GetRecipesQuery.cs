using Ernaehrbar.Parts.ReadModels;
using MediatR;

namespace Ernaehrbar.Parts.Queries;

/// <summary>
/// Query zum Abrufen einer Liste von Rezepten (mit optionaler Filterung).
/// </summary>
public record GetRecipesQuery(
    int GroupId,
    List<int>? TagIds = null,
    string? SearchTerm = null,
    int? Skip = null,
    int? Take = null) : IRequest<List<RecipeReadModel>>;
