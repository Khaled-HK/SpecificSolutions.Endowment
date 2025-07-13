using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Regions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.GetRegion
{
    public record GetRegionQuery(Guid Id) : IRequest<EndowmentResponse<FilterRegionDTO>>;
}
