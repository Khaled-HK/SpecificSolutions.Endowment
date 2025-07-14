using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Facilities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.GetFacility
{
    public record GetFacilityQuery(Guid Id) : IRequest<EndowmentResponse<FacilityDTO>>;
} 