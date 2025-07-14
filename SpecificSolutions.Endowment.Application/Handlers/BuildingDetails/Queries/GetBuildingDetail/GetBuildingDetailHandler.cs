using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Queries.GetBuildingDetail
{
    public class GetBuildingDetailHandler : IRequestHandler<GetBuildingDetailQuery, EndowmentResponse<BuildingDetailDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBuildingDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<BuildingDetailDTO>> Handle(GetBuildingDetailQuery request, CancellationToken cancellationToken)
        {
            var buildingDetail = await _unitOfWork.BuildingDetails.GetByIdAsync(request.Id, cancellationToken);
            if (buildingDetail == null)
            {
                return Response.FailureResponse<BuildingDetailDTO>("The specified building detail could not be located. Please verify the ID and try again.");
            }

            var buildingDetailDTO = new BuildingDetailDTO
            {
                Id = buildingDetail.Id,
                Name = buildingDetail.Name,
                WithinMosqueArea = buildingDetail.WithinMosqueArea,
                Floors = buildingDetail.Floors,
                BuildingCategory = buildingDetail.BuildingCategory,
                BuildingId = buildingDetail.BuildingId
            };

            return new(data: buildingDetailDTO);
        }
    }
} 