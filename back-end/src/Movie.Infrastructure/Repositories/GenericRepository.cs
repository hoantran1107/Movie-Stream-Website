using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Movie.Application.Abstractions;
using Movie.Infrastructure.Extensions;
using Movie.Infrastructure.Models;

namespace Movie.Infrastructure.Repositories;

public sealed class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext dbContext)
    {
        Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = Context.Set<TEntity>();
    }

    private DbContext Context { get; }

    public TEntity? GetById(object id)
    {
        ArgumentNullException.ThrowIfNull(id);
        return _dbSet.Find(id);
    }

    public async Task<TEntity?> GetByIdAsync(object id, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await _dbSet.FindAsync(id);
    }

    public IQueryable<TEntity > GetAll()
    {
        return _dbSet.AsQueryable();
    }

    public void Add(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbSet.Add(entity);
    }

    public async Task AddAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _dbSet.AddAsync(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities, bool isEnableAutoDetectChanges = false)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (!isEnableAutoDetectChanges)
        {
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
            _dbSet.AddRange(entities);
            Context.ChangeTracker.AutoDetectChangesEnabled = true;
        }
        else
        {
            _dbSet.AddRange(entities);
        }
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, bool isEnableAutoDetectChanges = false)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (!isEnableAutoDetectChanges)
        {
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
            await _dbSet.AddRangeAsync(entities);
            Context.ChangeTracker.AutoDetectChangesEnabled = true;
        }
        else
        {
            await _dbSet.AddRangeAsync(entities);
        }
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        _dbSet.UpdateRange(entities);
    }
    public void Delete(object id)
    {
        var entityToDelete = _dbSet.Find(id);
        Delete(entityToDelete);
    }

    public void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (Context.Entry(entity).State == EntityState.Detached)
            _dbSet.Attach(entity);

        _dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        _dbSet.RemoveRange(entities);
    }
}