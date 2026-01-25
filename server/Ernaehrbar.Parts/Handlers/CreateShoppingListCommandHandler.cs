using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Ports;
using MediatR;

namespace Ernaehrbar.Parts.Handlers;

/// <summary>
/// Handler für CreateShoppingListCommand.
/// </summary>
public class CreateShoppingListCommandHandler : IRequestHandler<CreateShoppingListCommand, CreateShoppingListResult>
{
    private readonly IShoppingListRepository _repository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    public CreateShoppingListCommandHandler(
        IShoppingListRepository repository,
        IGroupRepository groupRepository,
        IUserRepository userRepository)
    {
        _repository = repository;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public async Task<CreateShoppingListResult> Handle(CreateShoppingListCommand request, CancellationToken cancellationToken)
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

        var itemDtos = request.Items?.Select((item, index) => new ShoppingListItemDto(
            Id: null,
            IngredientName: item.IngredientName,
            Quantity: item.Quantity,
            Unit: item.Unit,
            RecipeIngredientId: item.RecipeIngredientId,
            IsChecked: false,
            Order: index
        )).ToList();

        var shoppingListDto = new ShoppingListDto(
            Id: null,
            GroupId: request.GroupId,
            CreatedByUserId: request.CreatedByUserId,
            Name: request.Name,
            ForWeekStartDate: request.ForWeekStartDate,
            ForWeekEndDate: request.ForWeekEndDate,
            Items: itemDtos
        );

        var shoppingListId = await _repository.AddAsync(shoppingListDto, cancellationToken);

        return new CreateShoppingListResult(shoppingListId);
    }
}
