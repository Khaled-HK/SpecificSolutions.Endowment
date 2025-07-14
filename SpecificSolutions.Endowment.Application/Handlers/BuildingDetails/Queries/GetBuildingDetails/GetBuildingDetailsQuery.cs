using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Queries.GetBuildingDetails
{
    public record GetBuildingDetailsQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 