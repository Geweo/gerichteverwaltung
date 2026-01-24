using Ernaehrbar.Parts.Ports;
using Ernaehrbar.Parts.Queries;
using MediatR;

namespace Ernaehrbar.Parts.Queries.Handlers;

/// <summary>
/// Handler für GetRecipeByIdQuery: ruft ein Rezept anhand der ID ab.
/// </summary>
public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, ReadModels.RecipeReadModel?>
{
    private readonly IRecipeReadRepository _readRepository;

    public GetRecipeByIdQueryHandler(IRecipeReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    /// <inheritdoc />
    public Task<ReadModels.RecipeReadModel?> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.GetByIdAsync(request.RecipeId, cancellationToken);
    }
}
