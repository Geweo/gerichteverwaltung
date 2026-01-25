using Ernaehrbar.Parts.Domain;
using MediatR;

namespace Ernaehrbar.Parts.Commands;

/// <summary>
/// Command zum Approven eines RecipeDraft (konvertiert zu Recipe).
/// </summary>
public record ApproveRecipeDraftCommand(
    int DraftId,
    int ApprovedByUserId
) : IRequest<ApproveRecipeDraftResult>;

/// <summary>
/// Result für ApproveRecipeDraftCommand.
/// </summary>
public record ApproveRecipeDraftResult(
    int RecipeId,
    int DraftId
);
