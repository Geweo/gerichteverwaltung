using Ernaehrbar.Parts.Commands;
using FluentValidation;

namespace Ernaehrbar.Parts.Validation;

/// <summary>
/// Validator für RegenerateRecipeCommand.
/// </summary>
public class RegenerateRecipeCommandValidator : AbstractValidator<RegenerateRecipeCommand>
{
    public RegenerateRecipeCommandValidator()
    {
        RuleFor(x => x.OriginalPrompt)
            .NotEmpty()
            .WithMessage("Original-Prompt darf nicht leer sein.")
            .MaximumLength(1000)
            .WithMessage("Original-Prompt darf maximal 1000 Zeichen lang sein.");

        RuleFor(x => x.NewPrompt)
            .MaximumLength(1000)
            .WithMessage("Neuer Prompt darf maximal 1000 Zeichen lang sein.")
            .When(x => x.NewPrompt is not null);

        RuleFor(x => x.ExistingTags)
            .NotNull()
            .WithMessage("ExistingTags darf nicht null sein.");
    }
}
