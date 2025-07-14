using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.GetChangeOfPathRequests
{
    public record GetChangeOfPathRequestsQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 