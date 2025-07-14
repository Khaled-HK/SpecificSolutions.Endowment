using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.FacilityDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.GetById
{
    public class GetFacilityDetailByIdHandler : IRequestHandler<GetFacilityDetailByIdQuery, EndowmentResponse<FacilityDetailDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFacilityDetailByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<FacilityDetailDTO>> Handle(GetFacilityDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var facilityDetail = await _unitOfWork.FacilityDetails.FindAsync(request.Id, cancellationToken: cancellationToken);
            if (facilityDetail == null)
            {
                return Response.FailureResponse<FacilityDetailDTO>("The specified Facility Detail could not be located. Please verify the Facility Detail ID and try again.");
            }

            var facilityDetailDTO = new FacilityDetailDTO
            {
                Id = facilityDetail.Id,
                Quantity = facilityDetail.Quantity,
            };

            return new(data: facilityDetailDTO);
        }
    }
}