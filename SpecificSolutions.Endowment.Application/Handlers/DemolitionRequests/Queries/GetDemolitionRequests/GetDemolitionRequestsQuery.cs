using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.GetDemolitionRequests
{
    public record GetDemolitionRequestsQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 