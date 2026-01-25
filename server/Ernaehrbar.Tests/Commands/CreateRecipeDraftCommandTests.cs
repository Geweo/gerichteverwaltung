using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Handlers;
using Ernaehrbar.Parts.Ports;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ernaehrbar.Tests.Commands;

/// <summary>
/// Tests für CreateRecipeDraftCommand (TDD).
/// </summary>
public class CreateRecipeDraftCommandTests
{
    [Fact]
    public async Task Handle_ShouldCreateRecipeDraft_WhenValidCommand()
    {
        var repository = Substitute.For<IRecipeDraftRepository>();
        var groupRepository = Substitute.For<IGroupRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        var groupId = 1;
        var userId = 1;
        var draftId = 100;

        groupRepository.GetByIdAsync(groupId, Arg.Any<CancellationToken>())
            .Returns(new GroupDto(groupId, "Test Group"));

        userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new UserDto(userId, "test-user", "test@example.com"));

        repository.AddAsync(Arg.Any<RecipeDraftDto>(), Arg.Any<CancellationToken>())
            .Returns(draftId);

        var handler = new CreateRecipeDraftCommandHandler(repository, groupRepository, userRepository);
        var command = new CreateRecipeDraftCommand(
            GroupId: groupId,
            CreatedByUserId: userId,
            Name: "Test Recipe",
            Source: RecipeSource.Generated,
            Description: "Test Description",
            Instructions: "Test Instructions",
            MealCategory: MealCategory.Lunch,
            Ingredients: new List<CreateRecipeDraftIngredientDto>
            {
                new("Tomaten", 500, "g"),
                new("Zwiebeln", 2, "Stück")
            }
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(draftId);
        result.Status.ShouldBe(DraftStatus.Pending);

        await repository.Received(1).AddAsync(
            Arg.Is<RecipeDraftDto>(d => 
                d.Name == "Test Recipe" &&
                d.Source == RecipeSource.Generated &&
                d.MealCategory == MealCategory.Lunch &&
                d.Ingredients != null &&
                d.Ingredients.Count == 2 &&
                d.Ingredients[0].Name == "Tomaten" &&
                d.Ingredients[0].Quantity == 500 &&
                d.Ingredients[0].Unit == "g"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenGroupNotFound()
    {
        var repository = Substitute.For<IRecipeDraftRepository>();
        var groupRepository = Substitute.For<IGroupRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        groupRepository.GetByIdAsync(999, Arg.Any<CancellationToken>())
            .Returns((GroupDto?)null);

        var handler = new CreateRecipeDraftCommandHandler(repository, groupRepository, userRepository);
        var command = new CreateRecipeDraftCommand(
            GroupId: 999,
            CreatedByUserId: 1,
            Name: "Test Recipe",
            Source: RecipeSource.Generated
        );

        await Should.ThrowAsync<InvalidOperationException>(
            () => handler.Handle(command, CancellationToken.None));

        await repository.DidNotReceive().AddAsync(Arg.Any<RecipeDraftDto>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        var repository = Substitute.For<IRecipeDraftRepository>();
        var groupRepository = Substitute.For<IGroupRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        groupRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(new GroupDto(1, "Test Group"));

        userRepository.GetByIdAsync(999, Arg.Any<CancellationToken>())
            .Returns((UserDto?)null);

        var handler = new CreateRecipeDraftCommandHandler(repository, groupRepository, userRepository);
        var command = new CreateRecipeDraftCommand(
            GroupId: 1,
            CreatedByUserId: 999,
            Name: "Test Recipe",
            Source: RecipeSource.Generated
        );

        await Should.ThrowAsync<InvalidOperationException>(
            () => handler.Handle(command, CancellationToken.None));

        await repository.DidNotReceive().AddAsync(Arg.Any<RecipeDraftDto>(), Arg.Any<CancellationToken>());
    }
}
