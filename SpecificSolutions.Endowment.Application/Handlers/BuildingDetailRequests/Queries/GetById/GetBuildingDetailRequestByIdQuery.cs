using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetailRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Queries.GetById
{
    public record GetBuildingDetailRequestByIdQuery(Guid Id) : IRequest<EndowmentResponse<BuildingDetailRequestDTO>>;
} 