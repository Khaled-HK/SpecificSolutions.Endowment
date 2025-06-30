using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Regions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.Filter
{
    public class FilterRegionHandler : IQueryHandler<FilterRegionQuery, PagedList<RegionDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterRegionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<RegionDTO>>> Handle(FilterRegionQuery query, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.Regions.GetByFilterAsync(query, cancellationToken);

            if (!requests.Items.Any())
            {
                return new EndowmentResponse<PagedList<RegionDTO>>(
                    PagedList<RegionDTO>.Empty());
            }

            return new EndowmentResponse<PagedList<RegionDTO>>(requests);
        }
    }
}