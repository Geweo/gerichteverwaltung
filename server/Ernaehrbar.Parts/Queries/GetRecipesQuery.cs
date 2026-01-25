using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Ports;
using Ernaehrbar.Parts.Queries.Common;
using Ernaehrbar.Parts.ReadModels;
using MediatR;

namespace Ernaehrbar.Parts.Queries;

/// <summary>
/// Query zum Abrufen einer Liste von Rezepten (mit optionaler Filterung und Pagination).
/// </summary>
public record GetRecipesQuery(
    int GroupId,
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    MealCategory? MealCategory = null,
    RecipeSource? Source = null,
    bool? Favorites = null,
    List<int>? TagIds = null,
    RecipeListSorting SortBy = RecipeListSorting.Name,
    SortDirectionEnum SortDirection = SortDirectionEnum.Asc) : IRequest<PaginatedResult<RecipeReadModel>>;
