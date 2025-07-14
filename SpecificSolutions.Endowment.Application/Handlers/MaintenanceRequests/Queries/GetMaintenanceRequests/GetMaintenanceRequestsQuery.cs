using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Queries.GetMaintenanceRequests
{
    public record GetMaintenanceRequestsQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 