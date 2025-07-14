using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Banks;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Banks.Queries.GetBank
{
    public record GetBankQuery(Guid Id) : IRequest<EndowmentResponse<BankDTO>>;
} 