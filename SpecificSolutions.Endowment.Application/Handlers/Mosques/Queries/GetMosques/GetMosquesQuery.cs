using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.GetMosques
{
    public record GetMosquesQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 