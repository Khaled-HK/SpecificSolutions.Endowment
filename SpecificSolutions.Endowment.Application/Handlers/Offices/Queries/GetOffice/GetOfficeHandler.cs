using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Offices;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.GetOffice
{
    public class GetOfficeHandler : IRequestHandler<GetOfficeQuery, EndowmentResponse<OfficeDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetOfficeHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<OfficeDTO>> Handle(GetOfficeQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Offices.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                return Response.FailureResponse<OfficeDTO>("The specified office could not be located. Please verify the ID and try again.");
            var dto = new OfficeDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = entity.Region.Name, // Using region name as location
                PhoneNumber = entity.PhoneNumber
            };
            return new(data: dto);
        }
    }
} 