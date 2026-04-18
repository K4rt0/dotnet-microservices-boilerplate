namespace MicroservicesBoilerplate.BuildingBlocks.Contracts.Pagination;

public sealed record CursorPageRequest<TCursor>(
    TCursor? After,
    int PageSize = 20,
    bool Ascending = true)
    where TCursor : struct, IComparable<TCursor>
{
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 200;

    public int NormalizedPageSize => PageSize switch
    {
        < 1 => DefaultPageSize,
        > MaxPageSize => MaxPageSize,
        _ => PageSize
    };
}