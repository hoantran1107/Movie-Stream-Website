namespace Movie.Application.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    ITransaction BeginTransaction();
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}