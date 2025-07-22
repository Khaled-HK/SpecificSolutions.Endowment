using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Mosques;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.GetMosque
{
    public class GetMosqueHandler : IRequestHandler<GetMosqueQuery, EndowmentResponse<MosqueDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetMosqueHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<MosqueDTO>> Handle(GetMosqueQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _unitOfWork.Mosques.GetByIdAsync(request.Id, cancellationToken);
                if (entity == null)
                    return Response.FailureResponse<MosqueDTO>("The specified mosque could not be located. Please verify the ID and try again.");

                if (entity.Building == null)
                    return Response.FailureResponse<MosqueDTO>("Building information for this mosque is not available.");

                var dto = new MosqueDTO
                {
                    MosqueID = entity.Id,
                    BuildingId = entity.BuildingId,
                    MosqueName = entity.Building.Name,
                    FileNumber = entity.Building.FileNumber,
                    MosqueDefinition = entity.MosqueDefinition,
                    MosqueClassification = entity.MosqueClassification,
                    Office = entity.Building.Office?.Name ?? string.Empty,
                    Unit = entity.Building.Unit,
                    Region = entity.Building.Region?.Name ?? string.Empty,
                    NearestLandmark = entity.Building.NearestLandmark,
                    ConstructionDate = entity.Building.ConstructionDate,
                    OpeningDate = entity.Building.OpeningDate,
                    MapLocation = entity.Building.MapLocation,
                    TotalLandArea = entity.Building.TotalLandArea,
                    TotalCoveredArea = entity.Building.TotalCoveredArea,
                    NumberOfFloors = entity.Building.NumberOfFloors,
                    Sanitation = entity.Building.Sanitation,
                    ElectricityMeter = entity.Building.ElectricityMeter,
                    AlternativeEnergySource = entity.Building.AlternativeEnergySource,
                    WaterSource = entity.Building.WaterSource,
                    BriefDescription = entity.Building.BriefDescription
                };

                return Response.FilterResponse(dto);

            }
            catch (Exception ex)
            {
                return Response.FailureResponse<MosqueDTO>($"An error occurred while retrieving the mosque: {ex.Message}");
            }
        }
    }
}