using Ernaehrbar.Parts.Commands;
using MediatR;

namespace Ernaehrbar.Parts.Handlers;

/// <summary>
/// Handler für ExportToBringCommand. Stub: wird mit IBringExporter implementiert.
/// </summary>
public class ExportToBringCommandHandler : IRequestHandler<ExportToBringCommand, Unit>
{
    /// <inheritdoc />
    public Task<Unit> Handle(ExportToBringCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Unit.Value);
    }
}
