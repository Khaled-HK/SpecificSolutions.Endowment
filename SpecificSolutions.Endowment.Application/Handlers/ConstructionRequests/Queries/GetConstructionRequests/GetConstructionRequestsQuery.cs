using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.GetConstructionRequests
{
    public record GetConstructionRequestsQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 