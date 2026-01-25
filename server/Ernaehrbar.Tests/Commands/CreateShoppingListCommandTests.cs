using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Handlers;
using Ernaehrbar.Parts.Ports;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ernaehrbar.Tests.Commands;

/// <summary>
/// Tests für CreateShoppingListCommand (TDD).
/// </summary>
public class CreateShoppingListCommandTests
{
    [Fact]
    public async Task Handle_ShouldCreateShoppingList_WhenValidCommand()
    {
        var repository = Substitute.For<IShoppingListRepository>();
        var groupRepository = Substitute.For<IGroupRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        var groupId = 1;
        var userId = 1;
        var shoppingListId = 100;

        groupRepository.GetByIdAsync(groupId, Arg.Any<CancellationToken>())
            .Returns(new GroupDto(groupId, "Test Group"));

        userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new UserDto(userId, "test-user", "test@example.com"));

        repository.AddAsync(Arg.Any<ShoppingListDto>(), Arg.Any<CancellationToken>())
            .Returns(shoppingListId);

        var handler = new CreateShoppingListCommandHandler(repository, groupRepository, userRepository);
        var command = new CreateShoppingListCommand(
            GroupId: groupId,
            CreatedByUserId: userId,
            Name: "Einkaufsliste Woche 1",
            Items: new List<CreateShoppingListItemDto>
            {
                new("Tomaten", 500, "g"),
                new("Zwiebeln", 2, "Stück")
            }
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(shoppingListId);

        await repository.Received(1).AddAsync(
            Arg.Is<ShoppingListDto>(sl =>
                sl.GroupId == groupId &&
                sl.CreatedByUserId == userId &&
                sl.Name == "Einkaufsliste Woche 1" &&
                sl.Items != null &&
                sl.Items.Count == 2 &&
                sl.Items[0].IngredientName == "Tomaten" &&
                sl.Items[0].Quantity == 500 &&
                sl.Items[0].Unit == "g"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCreateShoppingList_WhenNoItemsProvided()
    {
        var repository = Substitute.For<IShoppingListRepository>();
        var groupRepository = Substitute.For<IGroupRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        var groupId = 1;
        var userId = 1;
        var shoppingListId = 100;

        groupRepository.GetByIdAsync(groupId, Arg.Any<CancellationToken>())
            .Returns(new GroupDto(groupId, "Test Group"));

        userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new UserDto(userId, "test-user", "test@example.com"));

        repository.AddAsync(Arg.Any<ShoppingListDto>(), Arg.Any<CancellationToken>())
            .Returns(shoppingListId);

        var handler = new CreateShoppingListCommandHandler(repository, groupRepository, userRepository);
        var command = new CreateShoppingListCommand(
            GroupId: groupId,
            CreatedByUserId: userId,
            Name: "Leere Einkaufsliste"
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(shoppingListId);

        await repository.Received(1).AddAsync(
            Arg.Is<ShoppingListDto>(sl =>
                sl.Items == null || sl.Items.Count == 0),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenGroupNotFound()
    {
        var repository = Substitute.For<IShoppingListRepository>();
        var groupRepository = Substitute.For<IGroupRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        groupRepository.GetByIdAsync(999, Arg.Any<CancellationToken>())
            .Returns((GroupDto?)null);

        var handler = new CreateShoppingListCommandHandler(repository, groupRepository, userRepository);
        var command = new CreateShoppingListCommand(
            GroupId: 999,
            CreatedByUserId: 1,
            Name: "Test List"
        );

        await Should.ThrowAsync<InvalidOperationException>(
            () => handler.Handle(command, CancellationToken.None));

        await repository.DidNotReceive().AddAsync(Arg.Any<ShoppingListDto>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        var repository = Substitute.For<IShoppingListRepository>();
        var groupRepository = Substitute.For<IGroupRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        groupRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(new GroupDto(1, "Test Group"));

        userRepository.GetByIdAsync(999, Arg.Any<CancellationToken>())
            .Returns((UserDto?)null);

        var handler = new CreateShoppingListCommandHandler(repository, groupRepository, userRepository);
        var command = new CreateShoppingListCommand(
            GroupId: 1,
            CreatedByUserId: 999,
            Name: "Test List"
        );

        await Should.ThrowAsync<InvalidOperationException>(
            () => handler.Handle(command, CancellationToken.None));

        await repository.DidNotReceive().AddAsync(Arg.Any<ShoppingListDto>(), Arg.Any<CancellationToken>());
    }
}
