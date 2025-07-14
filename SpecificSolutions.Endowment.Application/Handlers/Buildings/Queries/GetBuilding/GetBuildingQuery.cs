using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Buildings;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.GetBuilding
{
    public record GetBuildingQuery(Guid Id) : IRequest<EndowmentResponse<BuildingDTO>>;
} 