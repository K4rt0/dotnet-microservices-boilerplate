using MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;
using MicroservicesBoilerplate.BuildingBlocks.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesBoilerplate.BuildingBlocks.Infrastructure.Repositories;

public interface IEfCoreRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    DbContext DbContext { get; }

    DbSet<TEntity> DbSet { get; }

    Task<DbContext> GetDbContextAsync();

    Task<DbSet<TEntity>> GetDbSetAsync();
}

public interface IEfCoreRepository<TEntity, TKey> : IEfCoreRepository<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{

}
