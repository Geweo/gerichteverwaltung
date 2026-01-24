using Ernaehrbar.Parts.Commands;
using MediatR;

namespace Ernaehrbar.Parts.Handlers;

/// <summary>
/// Handler für UploadRecipeCommand. Stub: wird mit IFileStorage/IRecipeStorage implementiert.
/// </summary>
public class UploadRecipeCommandHandler : IRequestHandler<UploadRecipeCommand, Unit>
{
    /// <inheritdoc />
    public Task<Unit> Handle(UploadRecipeCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Unit.Value);
    }
}
