using MicroservicesBoilerplate.BuildingBlocks.Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MicroservicesBoilerplate.BuildingBlocks.Infrastructure.Data;

public sealed class UnitOfWork<TContext> : IUnitOfWork
     where TContext : DbContext
{
    private readonly TContext _dbContext;
    private IDbContextTransaction? _currentTransaction;
    private bool _disposed;

    public UnitOfWork(TContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    private async Task DisposeCurrentTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    private void CheckDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(UnitOfWork<TContext>));
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        CheckDisposed();

        if (_currentTransaction != null)
            throw new InvalidOperationException("A transaction is already in progress.");

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        CheckDisposed();

        if (_currentTransaction == null)
            throw new InvalidOperationException("No active transaction to commit.");

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await DisposeCurrentTransactionAsync();
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        CheckDisposed();

        if (_currentTransaction == null)
            return;

        try
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await DisposeCurrentTransactionAsync();
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        _currentTransaction?.Dispose();
        _currentTransaction = null;
        _dbContext.Dispose();
        _disposed = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        await DisposeCurrentTransactionAsync();
        await _dbContext.DisposeAsync();
        _disposed = true;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        CheckDisposed();
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}