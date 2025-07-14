using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Queries.GetAccountDetails
{
    public record GetAccountDetailsQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 