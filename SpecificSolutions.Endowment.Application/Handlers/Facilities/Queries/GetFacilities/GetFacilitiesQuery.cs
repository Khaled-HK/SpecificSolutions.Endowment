using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.GetFacilities
{
    public record GetFacilitiesQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 