using Microsoft.EntityFrameworkCore;
using Movie.Application.Abstractions;

namespace Movie.Infrastructure.Repositories;

public class RepositoryFactory<TEntity>(DbContext context) : IRepositoryFactory where TEntity : class
{
    private DbContext Context { get; set; } = context ?? throw new ArgumentNullException(nameof(context));
    private bool disposed;

    /// <summary>
    ///     Gets the specified repository for the <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="hasCustomRepository"><c>True</c> if providing custom repositry</param>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}" /> interface.</returns>
    public IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
    {
        return new GenericRepository<TEntity>(Context);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Context.Dispose();
            }
        }
    }
}