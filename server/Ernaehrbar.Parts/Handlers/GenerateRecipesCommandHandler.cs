using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Models;
using Ernaehrbar.Parts.Ports;
using MediatR;

namespace Ernaehrbar.Parts.Handlers;

/// <summary>
/// Handler für GenerateRecipesCommand: erzeugt einen Wochenplan mit Rezepten via ILLMService.
/// </summary>
public class GenerateRecipesCommandHandler : IRequestHandler<GenerateRecipesCommand, MealPlanResult>
{
    private readonly ILLMService _llmService;

    public GenerateRecipesCommandHandler(ILLMService llmService)
    {
        _llmService = llmService;
    }

    /// <inheritdoc />
    public async Task<MealPlanResult> Handle(GenerateRecipesCommand request, CancellationToken cancellationToken)
    {
        // Validierung erfolgt über FluentValidation (GenerateRecipesCommandValidator)

        var tags = await _llmService.ExtractTagsFromPromptAsync(request.Prompt, cancellationToken);

        var recipes = await _llmService.GenerateRecipesAsync(
            request.Prompt,
            request.MealCategories,
            request.NumberOfDays,
            cancellationToken);

        return new MealPlanResult
        {
            Prompt = request.Prompt,
            Tags = tags,
            Recipes = recipes,
            MealCategories = request.MealCategories,
            NumberOfDays = request.NumberOfDays,
            GeneratedAt = DateTime.UtcNow,
        };
    }
}
