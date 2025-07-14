using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.GetQuranicSchools
{
    public record GetQuranicSchoolsQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 