using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Repositories;

public interface IReadOnlyRepository<TEntity> : IReadOnlyBasicRepository<TEntity>
{
    Task<List<TEntity>> GetListAsync(
        [NotNull] Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        CancellationToken cancellationToken = default);
}

public interface IReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{

}