using MediatR;

namespace Movie.Application.Common.Interfaces;

/// <summary>
/// Marker interface for queries (read operations)
/// </summary>
/// <typeparam name="TResponse">The response type</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
