using System.Linq.Expressions;

namespace Movie.Application.Abstractions;

public interface IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    ///     Get an entity in DB by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TEntity? GetById(object id);
    Task <TEntity?> GetByIdAsync(object id, CancellationToken ct = default);
    IQueryable<TEntity> GetAll();
    /// <summary>
    ///     Add an entity in DB (using Find / FindAsync).
    /// </summary>
    /// <param name="entity"></param>
    void Add(TEntity entity);

    /// <summary>
    ///     Add async an entity in DB (using Find / FindAsync).
    /// </summary>
    /// <param name="entity"></param>
    Task AddAsync(TEntity entity);

    /// <summary>
    ///     Add the entities in DB (using Find / FindAsync).
    /// </summary>
    /// <param name="entities"></param>
    void AddRange(IEnumerable<TEntity> entities, bool isEnableAutoDetectChanges = false);

    /// <summary>
    ///     Add async the entities in DB (using Find / FindAsync).
    /// </summary>
    /// <param name="entities"></param>
    Task AddRangeAsync(IEnumerable<TEntity> entities, bool isEnableAutoDetectChanges = false);

    /// <summary>
    ///     Update the entity in DB (using Find / FindAsync).
    /// </summary>
    /// <param name="entityToUpdate"></param>
    void Update(TEntity entityToUpdate);

    /// <summary>
    ///     Update the entities in DB (using Find / FindAsync).
    /// </summary>
    /// <param name="entity"></param>
    void UpdateRange(IEnumerable<TEntity> entities);

    /// <summary>
    ///     Delete the entity in DB by Id (using Find / FindAsync).
    /// </summary>
    /// <param name="id"></param>
    void Delete(object id);

    /// <summary>
    ///     Delete the entity in DB (using Find / FindAsync).
    /// </summary>
    /// <param name="entityToDelete"></param>
    void Delete(TEntity entityToDelete);

    /// <summary>
    ///     Delete the entities in DB (using Find / FindAsync).
    /// </summary>
    /// <param name="entities"></param>
    void DeleteRange(IEnumerable<TEntity> entities);
}