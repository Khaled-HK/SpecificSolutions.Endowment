using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.GetEntities
{
    public record GetEntitiesQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 