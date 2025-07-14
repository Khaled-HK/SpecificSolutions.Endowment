using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Queries.GetBuildingDetail
{
    public record GetBuildingDetailQuery(Guid Id) : IRequest<EndowmentResponse<BuildingDetailDTO>>;
} 