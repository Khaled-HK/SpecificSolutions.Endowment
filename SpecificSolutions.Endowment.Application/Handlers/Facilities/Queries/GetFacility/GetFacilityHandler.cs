using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Facilities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.GetFacility
{
    public class GetFacilityHandler : IRequestHandler<GetFacilityQuery, EndowmentResponse<FacilityDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetFacilityHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<FacilityDTO>> Handle(GetFacilityQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Facilities.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                return Response.FailureResponse<FacilityDTO>("The specified facility could not be located. Please verify the ID and try again.");
            var dto = new FacilityDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = entity.Location,
                ContactInfo = entity.ContactInfo,
                Capacity = entity.Capacity,
                Status = entity.Status
            };
            return new(data: dto);
        }
    }
} 