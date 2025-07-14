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
            var quranicSchools = await _unitOfWork.QuranicSchools.GetAllAsync(cancellationToken);

            var filteredQuranicSchools = quranicSchools
                .Where(q => q.Building.Name.Contains(request.SearchTerm))
                .Select(q => new QuranicSchoolDTO
                {
                    Name = q.Building.Name
                }
                );

            var pagedList = await PagedList<QuranicSchoolDTO>.CreateAsync(filteredQuranicSchools.AsQueryable(), request.PageNumber, request.PageSize, cancellationToken);

            return new EndowmentResponse<PagedList<QuranicSchoolDTO>>(pagedList);

        }
    }
}