using MediatR;

namespace Movie.Application.Common.Interfaces;

/// <summary>
/// Handler for commands that don't return a value
/// </summary>
/// <typeparam name="TCommand">The command type</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
}

/// <summary>
/// Handler for commands that return a value
/// </summary>
/// <typeparam name="TCommand">The command type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}
