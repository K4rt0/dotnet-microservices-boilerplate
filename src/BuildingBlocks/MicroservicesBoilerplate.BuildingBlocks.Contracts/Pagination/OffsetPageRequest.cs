namespace MicroservicesBoilerplate.BuildingBlocks.Contracts.Pagination;

public sealed record OffsetPageRequest(
    int PageNumber = 1,
    int PageSize = 20,
    string? Sorting = null,
    bool IncludeTotalCount = false)
{
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 200;

    public int NormalizedPageNumber => PageNumber < 1 ? 1 : PageNumber;

    public int NormalizedPageSize => PageSize switch
    {
        < 1 => DefaultPageSize,
        > MaxPageSize => MaxPageSize,
        _ => PageSize
    };

    public int SkipCount => (NormalizedPageNumber - 1) * NormalizedPageSize;
}