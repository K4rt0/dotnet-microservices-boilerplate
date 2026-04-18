using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesBoilerplate.BuildingBlocks.Infrastructure.Repositories;

public class EfCoreRepository<TDbContext, TEntity, TKey> : IEfCoreRepository<TEntity, TKey>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    protected readonly TDbContext _context;

    public EfCoreRepository(TDbContext context)
    {
        _context = context;
    }

    #region DbContext and DbSet Properties

    public DbContext DbContext => _context;

    public DbSet<TEntity> DbSet => _context.Set<TEntity>();

    #endregion

    #region IReadOnlyBasicRepository Methods

    public async Task<List<TEntity>> GetListAsync(bool tracking = false, CancellationToken cancellationToken = default)
    {
        var query = tracking ? DbSet : DbSet.AsNoTracking();
        return await query.ToListAsync(cancellationToken);
    }

    public Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        => DbSet.LongCountAsync(cancellationToken);

    public async Task<List<TEntity>> GetPagedListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? DbSet : DbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(sorting))
            query = ApplySorting(query, sorting);

        return await query
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync(cancellationToken);
    }

    private static IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, string sorting)
    {
        var parts = sorting.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
            return query;

        var propertyName = parts[0];
        var property = typeof(TEntity).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

        if (property is null)
            throw new ArgumentException($"Sorting property '{propertyName}' does not exist on {typeof(TEntity).Name}.", nameof(sorting));

        if (parts.Length > 1 &&
            !parts[1].Equals("ASC", StringComparison.OrdinalIgnoreCase) &&
            !parts[1].Equals("DESC", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Sorting direction must be ASC or DESC.", nameof(sorting));
        }

        var desc = parts.Length > 1 && parts[1].Equals("DESC", StringComparison.OrdinalIgnoreCase);
        var normalizedPropertyName = property.Name;

        return desc
            ? query.OrderByDescending(x => EF.Property<object>(x, normalizedPropertyName))
            : query.OrderBy(x => EF.Property<object>(x, normalizedPropertyName));
    }

    public async Task<TEntity> GetAsync(TKey id, bool tracking = false, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, tracking, cancellationToken);

        if (entity == null)
            throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} with Id '{id}' not found.");

        return entity;
    }

    public async Task<TEntity?> FindAsync(TKey id, bool tracking = false, CancellationToken cancellationToken = default)
    {
        var query = tracking ? DbSet : DbSet.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    #endregion

    #region IReadOnlyRepository Methods

    public async Task<List<TEntity>> GetListAsync(
        [NotNull] Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = tracking ? DbSet : DbSet.AsNoTracking();
        return await query.Where(predicate).ToListAsync(cancellationToken);
    }

    #endregion

    #region IBasicRepository Methods

    public async Task<TEntity> InsertAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await DbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task InsertManyAsync([NotNull] IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await DbSet.AddRangeAsync(entities, cancellationToken);
    }

    public Task<TEntity> UpdateAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        DbSet.Update(entity);
        return Task.FromResult(entity);
    }

    public Task UpdateManyAsync([NotNull] IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        DbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteManyAsync([NotNull] IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        DbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, tracking: false, cancellationToken: cancellationToken);

        if (entity == null)
            throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} with Id '{id}' not found.");

        await DeleteAsync(entity, cancellationToken);
    }

    public async Task DeleteManyAsync([NotNull] IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);

        var distinctIds = ids.Distinct().ToArray();

        if (distinctIds.Length == 0)
            return;

        var entities = await DbSet
            .Where(e => distinctIds.Contains(e.Id))
            .ToListAsync(cancellationToken);

        await DeleteManyAsync(entities, cancellationToken);
    }

    #endregion

    #region IRepository Methods

    public async Task<TEntity?> FindAsync(
        [NotNull] Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var query = tracking ? DbSet : DbSet.AsNoTracking();
        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity> GetAsync(
        [NotNull] Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(predicate, tracking, cancellationToken);

        if (entity == null)
            throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} not found matching the predicate.");

        return entity;
    }

    public async Task DeleteDirectAsync(
        [NotNull] Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        await DbSet
            .Where(predicate)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        [NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var entities = await DbSet
            .Where(predicate)
            .ToListAsync(cancellationToken);

        if (entities.Any())
            await DeleteManyAsync(entities, cancellationToken);
    }

    #endregion
}