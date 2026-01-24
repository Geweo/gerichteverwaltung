using Ernaehrbar.Parts.Ports;
using Ernaehrbar.Parts.Queries;
using MediatR;

namespace Ernaehrbar.Parts.Queries.Handlers;

/// <summary>
/// Handler für GetRecipesQuery: ruft eine Liste von Rezepten ab.
/// </summary>
public class GetRecipesQueryHandler : IRequestHandler<GetRecipesQuery, List<ReadModels.RecipeReadModel>>
{
    private readonly IRecipeReadRepository _readRepository;

    public GetRecipesQueryHandler(IRecipeReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    /// <inheritdoc />
    public Task<List<ReadModels.RecipeReadModel>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.GetRecipesAsync(
            request.GroupId,
            request.TagIds,
            request.SearchTerm,
            request.Skip,
            request.Take,
            cancellationToken);
    }
}
