using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

namespace MicroservicesBoilerplate.BuildingBlocks.Contracts.Repositories;

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>, IBasicRepository<TEntity>
{
    Task<TEntity?> FindAsync([NotNull] Expression<Func<TEntity, bool>> predicate, bool tracking = true, CancellationToken cancellationToken = default);
    Task<TEntity> GetAsync([NotNull] Expression<Func<TEntity, bool>> predicate, bool tracking = true, CancellationToken cancellationToken = default);
    Task DeleteDirectAsync([NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task DeleteAsync([NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}

public interface IRepository<TEntity, TKey> : IRepository<TEntity>, IReadOnlyRepository<TEntity, TKey>, IBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
}