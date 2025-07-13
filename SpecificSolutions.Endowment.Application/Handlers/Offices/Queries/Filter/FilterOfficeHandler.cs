using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Offices;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.Filter
{
    public class FilterOfficeHandler : IQueryHandler<FilterOfficeQuery, PagedList<FilterOfficeDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterOfficeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<FilterOfficeDTO>>> Handle(FilterOfficeQuery query, CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.Offices.GetByFilterAsync(query, cancellationToken);

            return new EndowmentResponse<PagedList<FilterOfficeDTO>>(pagedList);
        }
    }
}