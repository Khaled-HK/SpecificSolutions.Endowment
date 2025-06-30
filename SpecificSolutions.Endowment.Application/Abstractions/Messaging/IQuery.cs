using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<EndowmentResponse<TResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
