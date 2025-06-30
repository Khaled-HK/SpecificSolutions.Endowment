using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Facilities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.Filter
{
    public class FilterFacilityHandler : IQueryHandler<FilterFacilityQuery, PagedList<FacilityDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterFacilityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<FacilityDTO>>> Handle(FilterFacilityQuery request, CancellationToken cancellationToken)
        {
            var facilities = await _unitOfWork.Facilities.GetAllAsync();
            var filteredFacilities = facilities
                .Where(f => f.Name.Contains(request.SearchTerm) || f.Location.Contains(request.SearchTerm))
                .Select(f => new FacilityDTO
                {
                    Id = f.Id,
                    Name = f.Name,
                    Location = f.Location,
                    ContactInfo = f.ContactInfo,
                    Capacity = f.Capacity,
                    Status = f.Status
                });

            var pagedList = await PagedList<FacilityDTO>.CreateAsync(filteredFacilities.AsQueryable(), request.PageNumber, request.PageSize, cancellationToken);

            return new EndowmentResponse<PagedList<FacilityDTO>>(pagedList);
        }
    }
}