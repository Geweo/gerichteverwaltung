using MediatR;

namespace Ernaehrbar.Parts.Commands;

/// <summary>
/// Command zum Export einer Einkaufsliste zu Bring.com.
/// TODO: Einkaufslisten-Daten wenn IBringExporter implementiert.
/// </summary>
public record ExportToBringCommand() : IRequest<Unit>;
