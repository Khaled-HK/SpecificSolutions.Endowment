using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.GetBuildings
{
    public record GetBuildingsQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 