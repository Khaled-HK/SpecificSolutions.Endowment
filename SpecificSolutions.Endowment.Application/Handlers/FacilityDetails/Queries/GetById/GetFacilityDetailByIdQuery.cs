using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.FacilityDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.GetById
{
    public record GetFacilityDetailByIdQuery(Guid Id) : IRequest<EndowmentResponse<FacilityDetailDTO>>;
} 