namespace MicroservicesBoilerplate.BuildingBlocks.Contracts.Pagination;

public sealed record CursorPageResult<TItem, TCursor>(
    IReadOnlyList<TItem> Items,
    TCursor? NextCursor,
    bool HasMore)
    where TCursor : struct, IComparable<TCursor>;