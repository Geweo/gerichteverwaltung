using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Ports;
using MediatR;

namespace Ernaehrbar.Parts.Handlers;

/// <summary>
/// Handler für ApproveRecipeDraftCommand.
/// </summary>
public class ApproveRecipeDraftCommandHandler : IRequestHandler<ApproveRecipeDraftCommand, ApproveRecipeDraftResult>
{
    private readonly IRecipeDraftRepository _draftRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUserRepository _userRepository;

    public ApproveRecipeDraftCommandHandler(
        IRecipeDraftRepository draftRepository,
        IRecipeRepository recipeRepository,
        IUserRepository userRepository)
    {
        _draftRepository = draftRepository;
        _recipeRepository = recipeRepository;
        _userRepository = userRepository;
    }

    public async Task<ApproveRecipeDraftResult> Handle(ApproveRecipeDraftCommand request, CancellationToken cancellationToken)
    {
        var draft = await _draftRepository.GetByIdAsync(request.DraftId, cancellationToken);
        if (draft == null)
        {
            throw new InvalidOperationException($"RecipeDraft with ID {request.DraftId} not found");
        }

        if (draft.Status != DraftStatus.Pending)
        {
            throw new InvalidOperationException($"RecipeDraft with ID {request.DraftId} is not in Pending status (current: {draft.Status})");
        }

        var user = await _userRepository.GetByIdAsync(request.ApprovedByUserId, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.ApprovedByUserId} not found");
        }

        // Create Recipe from Draft
        var recipeIngredientDtos = draft.Ingredients?.Select((ing, index) => new RecipeIngredientDto(
            Id: null,
            Name: ing.Name,
            Quantity: ing.Quantity,
            Unit: ing.Unit,
            Notes: ing.Notes,
            Order: index
        )).ToList();

        var recipeDto = new RecipeDto(
            Id: null,
            GroupId: draft.GroupId,
            Name: draft.Name,
            Source: draft.Source,
            Description: draft.Description,
            Instructions: draft.Instructions,
            MealCategory: draft.MealCategory,
            Ingredients: recipeIngredientDtos
        );

        var recipeId = await _recipeRepository.AddAsync(recipeDto, cancellationToken);

        // Update Draft to Approved
        var updatedDraft = draft with
        {
            Status = DraftStatus.Approved,
            ReviewedByUserId = request.ApprovedByUserId,
            ReviewedAt = DateTime.UtcNow
        };

        await _draftRepository.UpdateAsync(updatedDraft, cancellationToken);

        return new ApproveRecipeDraftResult(recipeId, draft.Id!.Value);
    }
}
