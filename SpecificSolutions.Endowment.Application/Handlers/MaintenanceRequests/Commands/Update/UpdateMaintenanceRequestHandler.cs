using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Update
{
    public class UpdateMaintenanceRequestHandler : ICommandHandler<UpdateMaintenanceRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMaintenanceRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateMaintenanceRequestCommand request, CancellationToken cancellationToken)
        {
            var maintenanceRequest = await _unitOfWork.MaintenanceRequests.GetByIdAsync(request.Id, cancellationToken);
            if (maintenanceRequest == null)
            {
                return Response.FailureResponse("Id", "Maintenance request not found.");
            }

            // تحديث خصائص MaintenanceRequest
            maintenanceRequest.UpdateDetails(
                maintenanceType: request.MaintenanceType,
                location: request.Location,
                estimatedCost: request.EstimatedCost,
                expectedStartDate: request.ExpectedStartDate,
                expectedEndDate: request.ExpectedEndDate
            );

            await _unitOfWork.MaintenanceRequests.UpdateAsync(maintenanceRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}