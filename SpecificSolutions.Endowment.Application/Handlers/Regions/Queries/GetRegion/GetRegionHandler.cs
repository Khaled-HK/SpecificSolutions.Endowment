using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Regions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.GetRegion
{
    public class GetRegionHandler : IRequestHandler<GetRegionQuery, EndowmentResponse<FilterRegionDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRegionHandler(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<FilterRegionDTO>> Handle(GetRegionQuery request, CancellationToken cancellationToken)
        {
            var region = await _unitOfWork.Regions.GetByIdAsync(request.Id, cancellationToken);
            if (region == null)
            {
                return Response.FailureResponse<FilterRegionDTO>("The specified Region could not be located. Please verify the Region ID and try again.");
            }

            var regionDTO = new FilterRegionDTO
            {
                Id = region.Id,
                Name = region.Name,
                Country = region.Country,
                CityId = region.CityId,
            };

            return new(data: regionDTO);
        }
    }
}