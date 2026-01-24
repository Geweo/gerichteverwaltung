using Ernaehrbar.Parts.ReadModels;
using MediatR;

namespace Ernaehrbar.Parts.Queries;

/// <summary>
/// Query zum Abrufen eines Rezepts anhand der ID.
/// </summary>
public record GetRecipeByIdQuery(int RecipeId) : IRequest<RecipeReadModel?>;
