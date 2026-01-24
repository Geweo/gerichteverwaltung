using Ernaehrbar.Parts.Ports;
using Ernaehrbar.Parts.Queries;
using MediatR;

namespace Ernaehrbar.Parts.Queries.Handlers;

/// <summary>
/// Handler für GetMealPlansQuery: ruft eine Liste von Wochenplänen ab.
/// </summary>
public class GetMealPlansQueryHandler : IRequestHandler<GetMealPlansQuery, List<ReadModels.MealPlanReadModel>>
{
    private readonly IMealPlanReadRepository _readRepository;

    public GetMealPlansQueryHandler(IMealPlanReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    /// <inheritdoc />
    public Task<List<ReadModels.MealPlanReadModel>> Handle(GetMealPlansQuery request, CancellationToken cancellationToken)
    {
        return _readRepository.GetMealPlansAsync(
            request.GroupId,
            request.StartDateFrom,
            request.StartDateTo,
            request.Skip,
            request.Take,
            cancellationToken);
    }
}
