namespace Ernaehrbar.Parts.Queries.Common;

/// <summary>
/// Paginated result for queries.
/// Contains page information and items.
/// </summary>
public record PaginatedResult<TPayload>(
    int Page,
    int PageSize,
    int TotalCount,
    List<TPayload> Items)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
