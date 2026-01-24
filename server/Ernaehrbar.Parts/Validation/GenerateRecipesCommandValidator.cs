using Ernaehrbar.Parts.Commands;
using FluentValidation;

namespace Ernaehrbar.Parts.Validation;

/// <summary>
/// Validator für GenerateRecipesCommand.
/// </summary>
public class GenerateRecipesCommandValidator : AbstractValidator<GenerateRecipesCommand>
{
    public GenerateRecipesCommandValidator()
    {
        RuleFor(x => x.Prompt)
            .NotEmpty()
            .WithMessage("Prompt darf nicht leer sein.")
            .MaximumLength(1000)
            .WithMessage("Prompt darf maximal 1000 Zeichen lang sein.");

        RuleFor(x => x.MealCategories)
            .NotEmpty()
            .WithMessage("Mindestens eine Mahlzeitenkategorie muss ausgewählt sein.");

        RuleFor(x => x.NumberOfDays)
            .InclusiveBetween(7, 21)
            .WithMessage("Anzahl der Tage muss zwischen 7 und 21 liegen.");
    }
}
