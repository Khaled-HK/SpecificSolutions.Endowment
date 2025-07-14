using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetailRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Queries.GetById
{
    public class GetBuildingDetailRequestByIdHandler : IRequestHandler<GetBuildingDetailRequestByIdQuery, EndowmentResponse<BuildingDetailRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBuildingDetailRequestByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<BuildingDetailRequestDTO>> Handle(GetBuildingDetailRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var buildingDetailRequest = await _unitOfWork.BuildingDetailRequests.FindAsync(request.Id, cancellationToken: cancellationToken);
            if (buildingDetailRequest == null)
            {
                return Response.FailureResponse<BuildingDetailRequestDTO>("The specified Building Detail Request could not be located. Please verify the Building Detail Request ID and try again.");
            }

            var buildingDetailRequestDTO = new BuildingDetailRequestDTO
            {
                Id = buildingDetailRequest.Id,
                RequestDetails = buildingDetailRequest.RequestDetails,
                RequestDate = buildingDetailRequest.RequestDate,
                BuildingDetailId = buildingDetailRequest.BuildingDetailId
            };

            return new(data: buildingDetailRequestDTO);
        }
    }
} 