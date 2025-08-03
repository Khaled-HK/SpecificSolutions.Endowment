using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.QuranicSchools;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.Filter
{
    public class FilterQuranicSchoolHandler : IQueryHandler<FilterQuranicSchoolQuery, PagedList<QuranicSchoolDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterQuranicSchoolHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<QuranicSchoolDTO>>> Handle(FilterQuranicSchoolQuery request, CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.QuranicSchools.GetByFilterAsync(request, cancellationToken);

            return Response.FilterResponse(pagedList);
        }
    }
}