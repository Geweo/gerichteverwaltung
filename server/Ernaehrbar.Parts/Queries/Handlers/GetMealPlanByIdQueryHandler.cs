using Ernaehrbar.Parts.Ports;
using Ernaehrbar.Parts.Queries;
using MediatR;

namespace Ernaehrbar.Parts.Queries.Handlers;

/// <summary>
/// Handler für GetMealPlanByIdQuery: ruft einen Wochenplan anhand der ID ab.
/// </summary>
public class GetMealPlanByIdQueryHandler : IRequestHandler<GetMealPlanByIdQuery, ReadModels.MealPlanReadModel?>
{
    private readonly IMealPlanReadRepository _readRepository;

    public GetMealPlanByIdQueryHandler(IMealPlanReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    /// <inheritdoc />
    public Task<ReadModels.MealPlanReadModel?> Handle(GetMealPlanByIdQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.GetByIdAsync(request.MealPlanId, cancellationToken);
    }
}
