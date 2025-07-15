using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Cities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.Filter
{
    public class FilterCityHandler : IQueryHandler<FilterCityQuery, PagedList<CityDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterCityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<CityDTO>>> Handle(FilterCityQuery query, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.Cities.GetByFilterAsync(query, cancellationToken);

            if (!requests.Items.Any())
            {
                return Response.FilterResponse(PagedList<CityDTO>.Empty());
            }

            return Response.FilterResponse(requests);
        }
    }
}