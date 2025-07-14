using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.AccountDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Queries.GetAccountDetail
{
    public record GetAccountDetailQuery(Guid Id) : IRequest<EndowmentResponse<AccountDetailDTO>>;
} 