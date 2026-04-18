namespace MicroservicesBoilerplate.BuildingBlocks.Contracts.Pagination;

public sealed record OffsetPageResult<TItem>(
    IReadOnlyList<TItem> Items,
    int PageNumber,
    int PageSize,
    long? TotalCount)
{
    public bool HasNextPage => TotalCount.HasValue && PageNumber * PageSize < TotalCount.Value;
    public bool HasPreviousPage => PageNumber > 1;
}