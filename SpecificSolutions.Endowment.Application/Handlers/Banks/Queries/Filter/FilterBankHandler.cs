using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Banks;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Banks.Queries.Filter
{
    public class FilterBankHandler : IQueryHandler<FilterBankQuery, PagedList<BankDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterBankHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<BankDTO>>> Handle(FilterBankQuery query, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.Banks.GetByFilterAsync(query, cancellationToken);

            if (!requests.Items.Any())
            {
                return new EndowmentResponse<PagedList<BankDTO>>(
                    PagedList<BankDTO>.Empty());
            }

            return new EndowmentResponse<PagedList<BankDTO>>(requests);
        }
    }
}