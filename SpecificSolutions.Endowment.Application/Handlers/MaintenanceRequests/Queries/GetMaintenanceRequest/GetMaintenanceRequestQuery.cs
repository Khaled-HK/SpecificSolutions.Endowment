using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.MaintenanceRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Queries.GetMaintenanceRequest
{
    public record GetMaintenanceRequestQuery(Guid Id) : IRequest<EndowmentResponse<MaintenanceRequestDTO>>;
} 