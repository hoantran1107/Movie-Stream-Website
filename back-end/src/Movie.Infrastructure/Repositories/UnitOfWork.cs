using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Movie.Application.Abstractions;

namespace Movie.Infrastructure.Repositories;

/// <summary>
///     Initializes a new instance of the <see cref="UnitOfWork{TContext}" /> class.
/// </summary>
/// <param name="context">The context.</param>
public class UnitOfWork<TContext>(TContext context) : IRepositoryFactory, IUnitOfWork where TContext : DbContext
{
    private bool disposed;
    private Dictionary<Type, object> repositories = [];

    /// <summary>
    ///     Gets the db context.
    /// </summary>
    /// <returns>The instance of type <typeparamref name="TContext" />.</returns>
    public TContext DbContext { get; } = context ?? throw new ArgumentNullException(nameof(context));

    /// <summary>
    ///     Gets the specified repository for the <typeparamref name="TEntity" />.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="hasCustomRepository"></param>
    /// <returns></returns>
    public IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
    {
        repositories ??= [];

        // what's the best way to support custom reposity?
        if (hasCustomRepository)
        {
            var customRepo = DbContext.GetService<IGenericRepository<TEntity>>();
            if (customRepo != null) return customRepo;
        }

        var type = typeof(TEntity);
        if (!repositories.TryGetValue(type, out var value))
        {
            value = new GenericRepository<TEntity>(DbContext);
            repositories[type] = value;
        }

        return (IGenericRepository<TEntity>)value;
    }

    public ITransaction BeginTransaction()
    {
        var tx = DbContext.Database.BeginTransaction();
        return new EfCoreTransaction(tx);
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var tx = await DbContext.Database.BeginTransactionAsync(cancellationToken);
        return new EfCoreTransaction(tx);
    }

    /// Summary:
    /// Saves all changes made in this context to the database.
    /// This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
    /// to discover any changes to entity instances before saving to the underlying database.
    /// This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
    /// 
    /// Returns:
    /// The number of state entries written to the database.
    /// 
    /// Exceptions:
    /// T:Microsoft.EntityFrameworkCore.DbUpdateException:
    /// An error is encountered while saving to the database.
    /// 
    /// T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException:
    /// A concurrency violation is encountered while saving to the database. A concurrency
    /// violation occurs when an unexpected number of rows are affected during save.
    /// This is usually because the data in the database has been modified since it was
    /// loaded into memory.
    public int SaveChanges()
    {
        return DbContext.SaveChanges();
    }

    /// <summary>
    ///     Saves all changes made in this context to the database.
    ///     This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges to
    ///     discover any changes to entity instances before saving to the underlying database.
    ///     This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
    ///     Multiple active operations on the same context instance are not supported.
    ///     Use 'await' to ensure that any asynchronous operations have completed before calling another method on this
    ///     context.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous save operation. The task result contains the number of state entries
    ///     written to the database.
    /// </returns>
    /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
    /// <exception cref="DbUpdateConcurrencyException">
    ///     A concurrency violation is encountered while saving to the database.
    ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
    ///     This is usually because the data in the database has been modified since it was loaded into memory.
    /// </exception>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
            if (disposing)
                DbContext.Dispose();

        disposed = true;
    }
}