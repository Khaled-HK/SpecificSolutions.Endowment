using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Buildings;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.GetBuilding
{
    public class GetBuildingHandler : IRequestHandler<GetBuildingQuery, EndowmentResponse<BuildingDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetBuildingHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<BuildingDTO>> Handle(GetBuildingQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Buildings.FindAsync(request.Id, cancellationToken);
            if (entity == null)
                return Response.FailureResponse<BuildingDTO>("The specified building could not be located. Please verify the ID and try again.");
            var dto = new BuildingDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                FileNumber = entity.FileNumber,
                Definition = entity.Definition,
                Classification = entity.Classification,
                Office = entity.Office?.Name,
                Unit = entity.Unit,
                Region = entity.Region?.Name,
                NearestLandmark = entity.NearestLandmark,
                ConstructionDate = entity.ConstructionDate,
                OpeningDate = entity.OpeningDate,
                MapLocation = entity.MapLocation,
                TotalLandArea = entity.TotalLandArea,
                TotalCoveredArea = entity.TotalCoveredArea,
                NumberOfFloors = entity.NumberOfFloors,
                ElectricityMeter = entity.ElectricityMeter,
                AlternativeEnergySource = entity.AlternativeEnergySource,
                WaterSource = entity.WaterSource,
                Sanitation = entity.Sanitation,
                BriefDescription = entity.BriefDescription,
                UserId = entity.UserId
            };
            return new(data: dto);
        }
    }
} 