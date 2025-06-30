using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.AccountDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Queries.Filter
{
    public class FilterAccountDetailHandler : IQueryHandler<FilterAccountDetailQuery, PagedList<FilterAccountDetailDTO>>
    {
        private readonly IAccountDetailRepository _accountDetailRepository;

        public FilterAccountDetailHandler(IAccountDetailRepository accountDetailRepository)
        {
            _accountDetailRepository = accountDetailRepository;
        }

        public async Task<EndowmentResponse<PagedList<FilterAccountDetailDTO>>> Handle(FilterAccountDetailQuery query, CancellationToken cancellationToken)
        {
            var accountDetails = await _accountDetailRepository.GetByFilterAsync(query, cancellationToken);

            return new EndowmentResponse<PagedList<FilterAccountDetailDTO>>(accountDetails);
        }
    }
}
