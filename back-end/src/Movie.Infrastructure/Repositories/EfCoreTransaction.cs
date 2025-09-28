using Microsoft.EntityFrameworkCore.Storage;
using Movie.Application.Abstractions;

namespace Movie.Infrastructure.Repositories;

public sealed class EfCoreTransaction(IDbContextTransaction transaction) : ITransaction
{
    private readonly IDbContextTransaction _transaction =
        transaction ?? throw new ArgumentNullException(nameof(transaction));

    public void Commit()
    {
        _transaction.Commit();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return _transaction.CommitAsync(cancellationToken);
    }

    public void Rollback()
    {
        _transaction.Rollback();
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return _transaction.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _transaction.DisposeAsync();
    }
}