using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Abstractions.Messaging
{
    public interface ICommand : IRequest<EndowmentResponse>, IBaseCommand;

    public interface ICommand<TResponse> : IRequest<EndowmentResponse<TResponse>>, IBaseCommand;

    public interface IBaseCommand;

}
