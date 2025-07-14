using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.ConstructionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.GetConstructionRequest
{
    public record GetConstructionRequestQuery(Guid Id) : IRequest<EndowmentResponse<ConstructionRequestDTO>>;
} 