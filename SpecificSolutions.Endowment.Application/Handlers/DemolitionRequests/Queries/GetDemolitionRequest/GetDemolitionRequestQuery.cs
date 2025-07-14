using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.DemolitionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.GetDemolitionRequest
{
    public record GetDemolitionRequestQuery(Guid Id) : IRequest<EndowmentResponse<DemolitionRequestDTO>>;
} 