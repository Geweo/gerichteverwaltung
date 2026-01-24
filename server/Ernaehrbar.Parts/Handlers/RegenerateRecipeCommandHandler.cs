using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Ports;
using MediatR;

namespace Ernaehrbar.Parts.Handlers;

/// <summary>
/// Handler für RegenerateRecipeCommand: regeneriert ein Rezept via ILLMService.
/// </summary>
public class RegenerateRecipeCommandHandler : IRequestHandler<RegenerateRecipeCommand, GeneratedRecipe>
{
    private readonly ILLMService _llmService;

    public RegenerateRecipeCommandHandler(ILLMService llmService)
    {
        _llmService = llmService;
    }

    /// <inheritdoc />
    public Task<GeneratedRecipe> Handle(RegenerateRecipeCommand request, CancellationToken cancellationToken)
    {
        return _llmService.RegenerateRecipeAsync(
            request.OriginalPrompt,
            request.NewPrompt,
            request.MealCategory,
            request.ExistingTags,
            cancellationToken);
    }
}
