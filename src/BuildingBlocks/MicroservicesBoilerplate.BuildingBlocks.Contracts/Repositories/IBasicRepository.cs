using System.Diagnostics.CodeAnalysis;
using MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

namespace MicroservicesBoilerplate.BuildingBlocks.Contracts.Repositories;

public interface IBasicRepository<TEntity> : IReadOnlyBasicRepository<TEntity>
{
    Task<TEntity> InsertAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);
    Task InsertManyAsync([NotNull] IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateManyAsync([NotNull] IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteManyAsync([NotNull] IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
}

public interface IBasicRepository<TEntity, TKey> : IBasicRepository<TEntity>, IReadOnlyBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
    Task DeleteManyAsync([NotNull] IEnumerable<TKey> ids, CancellationToken cancellationToken = default);
}