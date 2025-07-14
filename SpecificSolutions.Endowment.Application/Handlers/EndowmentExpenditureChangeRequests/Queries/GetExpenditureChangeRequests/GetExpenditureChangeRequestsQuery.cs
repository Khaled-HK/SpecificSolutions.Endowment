using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Queries.GetExpenditureChangeRequests
{
    public record GetExpenditureChangeRequestsQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 