using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Ports;
using MediatR;

namespace Ernaehrbar.Parts.Handlers;

/// <summary>
/// Handler für CreateRecipeDraftCommand.
/// </summary>
public class CreateRecipeDraftCommandHandler : IRequestHandler<CreateRecipeDraftCommand, CreateRecipeDraftResult>
{
    private readonly IRecipeDraftRepository _repository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    public CreateRecipeDraftCommandHandler(
        IRecipeDraftRepository repository,
        IGroupRepository groupRepository,
        IUserRepository userRepository)
    {
        _repository = repository;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public async Task<CreateRecipeDraftResult> Handle(CreateRecipeDraftCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
        if (group == null)
        {
            throw new InvalidOperationException($"Group with ID {request.GroupId} not found");
        }

        var user = await _userRepository.GetByIdAsync(request.CreatedByUserId, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.CreatedByUserId} not found");
        }

        var ingredientDtos = request.Ingredients?.Select((ing, index) => new RecipeDraftIngredientDto(
            Id: null,
            Name: ing.Name,
            Quantity: ing.Quantity,
            Unit: ing.Unit,
            Notes: ing.Notes,
            Order: index
        )).ToList();

        var draftDto = new RecipeDraftDto(
            Id: null,
            GroupId: request.GroupId,
            CreatedByUserId: request.CreatedByUserId,
            Name: request.Name,
            Source: request.Source,
            Status: DraftStatus.Pending,
            Description: request.Description,
            Instructions: request.Instructions,
            MealCategory: request.MealCategory,
            OriginalData: request.OriginalData,
            Ingredients: ingredientDtos
        );

        var draftId = await _repository.AddAsync(draftDto, cancellationToken);

        return new CreateRecipeDraftResult(draftId, DraftStatus.Pending);
    }
}
