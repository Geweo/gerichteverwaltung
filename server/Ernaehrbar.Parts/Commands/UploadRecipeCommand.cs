using MediatR;

namespace Ernaehrbar.Parts.Commands;

/// <summary>
/// Command zum Hochladen und Verarbeiten eines Rezepts (PDF/Bild).
/// TODO: Stream/bytes, Dateiname wenn IFileStorage/Upload implementiert.
/// </summary>
public record UploadRecipeCommand() : IRequest<Unit>;
