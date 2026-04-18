using MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;
using MicroservicesBoilerplate.BuildingBlocks.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroservicesBoilerplate.BuildingBlocks.Infrastructure.Repositories;

public interface IEfCoreRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    DbContext DbContext { get; }

    DbSet<TEntity> DbSet { get; }
}

public interface IEfCoreRepository<TEntity, TKey> : IEfCoreRepository<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{

}
