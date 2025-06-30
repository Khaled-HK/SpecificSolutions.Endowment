using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Accounts;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.GetAccount
{
    public record GetAccountQuery(Guid Id) : IRequest<EndowmentResponse<FilterAccountDTO>>;

}