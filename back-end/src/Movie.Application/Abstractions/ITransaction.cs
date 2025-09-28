namespace Movie.Application.Abstractions;

public interface ITransaction : IDisposable, IAsyncDisposable
{
    void Commit();
    Task CommitAsync(CancellationToken cancellationToken = default);
    void Rollback();
    Task RollbackAsync(CancellationToken cancellationToken = default);
}