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
            var entity = await _unitOfWork.Mosques.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                return Response.FailureResponse<MosqueDTO>("The specified mosque could not be located. Please verify the ID and try again.");
            var dto = new MosqueDTO
            {
                MosqueID = entity.Id,
                MosqueName = entity.Building.Name,
                FileNumber = entity.Building.FileNumber,
                MosqueDefinition = entity.MosqueDefinition,
                MosqueClassification = entity.MosqueClassification,
                Unit = entity.Building.Unit,
                NearestLandmark = entity.Building.NearestLandmark,
                MapLocation = entity.Building.MapLocation,
                Sanitation = entity.Building.Sanitation,
                ElectricityMeter = entity.Building.ElectricityMeter,
                AlternativeEnergySource = entity.Building.AlternativeEnergySource,
                WaterSource = entity.Building.WaterSource,
                BriefDescription = entity.Building.BriefDescription
            };
            return new(data: dto);
        }
    }
} 