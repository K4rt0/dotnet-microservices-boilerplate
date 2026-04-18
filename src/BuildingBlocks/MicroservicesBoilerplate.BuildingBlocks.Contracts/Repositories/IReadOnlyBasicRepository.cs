using MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

namespace MicroservicesBoilerplate.BuildingBlocks.Contracts.Repositories;

public interface IReadOnlyBasicRepository<TEntity>
{
    Task<List<TEntity>> GetListAsync(bool tracking = true, CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetPagedListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        bool tracking = true,
        CancellationToken cancellationToken = default);
}

public interface IReadOnlyBasicRepository<TEntity, TKey> : IReadOnlyBasicRepository<TEntity>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    Task<TEntity> GetAsync(TKey id, bool tracking = true, CancellationToken cancellationToken = default);
    Task<TEntity?> FindAsync(TKey id, bool tracking = true, CancellationToken cancellationToken = default);
    // Task<CursorPageRequest> GetCursorPagedListAsync
}