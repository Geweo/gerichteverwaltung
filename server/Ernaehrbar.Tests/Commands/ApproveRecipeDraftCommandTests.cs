using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Handlers;
using Ernaehrbar.Parts.Ports;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ernaehrbar.Tests.Commands;

/// <summary>
/// Tests für ApproveRecipeDraftCommand (TDD).
/// </summary>
public class ApproveRecipeDraftCommandTests
{
    [Fact]
    public async Task Handle_ShouldApproveDraftAndCreateRecipe_WhenValidDraft()
    {
        var draftRepository = Substitute.For<IRecipeDraftRepository>();
        var recipeRepository = Substitute.For<IRecipeRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        var draftId = 100;
        var userId = 1;
        var groupId = 1;
        var recipeId = 200;

        var existingDraft = new RecipeDraftDto(
            Id: draftId,
            GroupId: groupId,
            CreatedByUserId: userId,
            Name: "Test Recipe",
            Source: RecipeSource.Generated,
            Status: DraftStatus.Pending,
            Description: "Test Description",
            Instructions: "Test Instructions",
            MealCategory: MealCategory.Lunch,
            Ingredients: new List<RecipeDraftIngredientDto>
            {
                new(null, "Tomaten", 500, "g", null, 0),
                new(null, "Zwiebeln", 2, "Stück", null, 1)
            }
        );

        draftRepository.GetByIdAsync(draftId, Arg.Any<CancellationToken>())
            .Returns(existingDraft);

        userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new UserDto(userId, "test-user", "test@example.com"));

        recipeRepository.AddAsync(Arg.Any<RecipeDto>(), Arg.Any<CancellationToken>())
            .Returns(recipeId);

        var handler = new ApproveRecipeDraftCommandHandler(
            draftRepository,
            recipeRepository,
            userRepository);

        var command = new ApproveRecipeDraftCommand(
            DraftId: draftId,
            ApprovedByUserId: userId
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.RecipeId.ShouldBe(recipeId);
        result.DraftId.ShouldBe(draftId);

        // Verify draft was updated to Approved
        await draftRepository.Received(1).UpdateAsync(
            Arg.Is<RecipeDraftDto>(d =>
                d.Id == draftId &&
                d.Status == DraftStatus.Approved &&
                d.ReviewedByUserId == userId &&
                d.ReviewedAt != null),
            Arg.Any<CancellationToken>());

        // Verify recipe was created
        await recipeRepository.Received(1).AddAsync(
            Arg.Is<RecipeDto>(r =>
                r.GroupId == groupId &&
                r.Name == "Test Recipe" &&
                r.Source == RecipeSource.Generated &&
                r.MealCategory == MealCategory.Lunch &&
                r.Ingredients != null &&
                r.Ingredients.Count == 2),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDraftNotFound()
    {
        var draftRepository = Substitute.For<IRecipeDraftRepository>();
        var recipeRepository = Substitute.For<IRecipeRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        draftRepository.GetByIdAsync(999, Arg.Any<CancellationToken>())
            .Returns((RecipeDraftDto?)null);

        var handler = new ApproveRecipeDraftCommandHandler(
            draftRepository,
            recipeRepository,
            userRepository);

        var command = new ApproveRecipeDraftCommand(
            DraftId: 999,
            ApprovedByUserId: 1
        );

        await Should.ThrowAsync<InvalidOperationException>(
            () => handler.Handle(command, CancellationToken.None));

        await recipeRepository.DidNotReceive().AddAsync(Arg.Any<RecipeDto>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDraftAlreadyApproved()
    {
        var draftRepository = Substitute.For<IRecipeDraftRepository>();
        var recipeRepository = Substitute.For<IRecipeRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        var draftId = 100;
        var existingDraft = new RecipeDraftDto(
            Id: draftId,
            GroupId: 1,
            CreatedByUserId: 1,
            Name: "Test Recipe",
            Source: RecipeSource.Generated,
            Status: DraftStatus.Approved, // Already approved
            ReviewedByUserId: 1,
            ReviewedAt: DateTime.UtcNow
        );

        draftRepository.GetByIdAsync(draftId, Arg.Any<CancellationToken>())
            .Returns(existingDraft);

        var handler = new ApproveRecipeDraftCommandHandler(
            draftRepository,
            recipeRepository,
            userRepository);

        var command = new ApproveRecipeDraftCommand(
            DraftId: draftId,
            ApprovedByUserId: 1
        );

        await Should.ThrowAsync<InvalidOperationException>(
            () => handler.Handle(command, CancellationToken.None));

        await recipeRepository.DidNotReceive().AddAsync(Arg.Any<RecipeDto>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        var draftRepository = Substitute.For<IRecipeDraftRepository>();
        var recipeRepository = Substitute.For<IRecipeRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        var draftId = 100;
        var existingDraft = new RecipeDraftDto(
            Id: draftId,
            GroupId: 1,
            CreatedByUserId: 1,
            Name: "Test Recipe",
            Source: RecipeSource.Generated,
            Status: DraftStatus.Pending
        );

        draftRepository.GetByIdAsync(draftId, Arg.Any<CancellationToken>())
            .Returns(existingDraft);

        userRepository.GetByIdAsync(999, Arg.Any<CancellationToken>())
            .Returns((UserDto?)null);

        var handler = new ApproveRecipeDraftCommandHandler(
            draftRepository,
            recipeRepository,
            userRepository);

        var command = new ApproveRecipeDraftCommand(
            DraftId: draftId,
            ApprovedByUserId: 999
        );

        await Should.ThrowAsync<InvalidOperationException>(
            () => handler.Handle(command, CancellationToken.None));

        await recipeRepository.DidNotReceive().AddAsync(Arg.Any<RecipeDto>(), Arg.Any<CancellationToken>());
    }
}
