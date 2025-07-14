using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Buildings;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.Filter
{
    public class FilterBuildingDetailHandler : IQueryHandler<FilterBuildingQuery, PagedList<BuildingDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterBuildingDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<BuildingDTO>>> Handle(FilterBuildingQuery request, CancellationToken cancellationToken)
        {
            var buildingDetails = await _unitOfWork.Buildings.GetAllAsync(cancellationToken);
            var filteredDetails = buildingDetails
                .Where(bd => bd.Name.Contains(request.SearchTerm))
                .Select(bd => new BuildingDTO
                {
                    Id = bd.Id,
                    Name = bd.Name,
                });

            var pagedList = await PagedList<BuildingDTO>.CreateAsync(filteredDetails.AsQueryable(), request.PageNumber, request.PageSize, cancellationToken);

            return new EndowmentResponse<PagedList<BuildingDTO>>(pagedList);
        }
    }
}