using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Abstractions.Messaging
{
    public interface ICommandHandler<in TCommand>
    : IRequestHandler<TCommand, EndowmentResponse>
    where TCommand : ICommand;

    public interface ICommandHandler<in TCommand, TResponse>
        : IRequestHandler<TCommand, EndowmentResponse<TResponse>>
        where TCommand : ICommand<TResponse>;

    //public interface IQueryHandler<in TQuery, TResponse>
    //    : IRequestHandler<TQuery, EndowmentResponse<TResponse>>
    //    where TQuery : IQuery<TResponse>;
}
