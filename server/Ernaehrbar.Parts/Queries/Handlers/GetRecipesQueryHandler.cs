using Ernaehrbar.Parts.Ports;
using Ernaehrbar.Parts.Queries;
using MediatR;

namespace Ernaehrbar.Parts.Queries.Handlers;

/// <summary>
/// Handler für GetRecipesQuery: ruft eine paginierte Liste von Rezepten ab.
/// </summary>
public class GetRecipesQueryHandler : IRequestHandler<GetRecipesQuery, Queries.Common.PaginatedResult<ReadModels.RecipeReadModel>>
{
    private readonly IRecipeReadRepository _readRepository;

    public GetRecipesQueryHandler(IRecipeReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    /// <inheritdoc />
    public Task<Queries.Common.PaginatedResult<ReadModels.RecipeReadModel>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.GetRecipesAsync(
            request.GroupId,
            request.Page,
            request.PageSize,
            request.SearchTerm,
            request.MealCategory,
            request.Source,
            request.Favorites,
            request.TagIds,
            request.SortBy,
            request.SortDirection,
            cancellationToken);
    }
}
