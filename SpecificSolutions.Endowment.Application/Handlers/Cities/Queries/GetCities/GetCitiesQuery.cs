using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.GetCities
{
    public record GetCitiesQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 