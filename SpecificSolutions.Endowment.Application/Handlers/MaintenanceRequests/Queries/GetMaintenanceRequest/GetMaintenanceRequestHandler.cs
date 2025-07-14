using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.MaintenanceRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Queries.GetMaintenanceRequest
{
    public class GetMaintenanceRequestHandler : IRequestHandler<GetMaintenanceRequestQuery, EndowmentResponse<MaintenanceRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetMaintenanceRequestHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<MaintenanceRequestDTO>> Handle(GetMaintenanceRequestQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.MaintenanceRequests.FindAsync(request.Id, cancellationToken);
            if (entity == null)
                return Response.FailureResponse<MaintenanceRequestDTO>("The specified maintenance request could not be located. Please verify the ID and try again.");
            var dto = new MaintenanceRequestDTO
            {
                Id = entity.Id,
                MaintenanceType = entity.MaintenanceType,
                EstimatedCost = entity.EstimatedCost,
                ExpectedStartDate = entity.ExpectedStartDate,
                ExpectedEndDate = entity.ExpectedEndDate
            };
            return new(data: dto);
        }
    }
} 