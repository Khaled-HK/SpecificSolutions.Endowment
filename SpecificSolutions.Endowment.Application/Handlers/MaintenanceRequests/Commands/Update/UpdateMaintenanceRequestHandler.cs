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
            //var maintenanceRequest = await _maintenanceRequestRepository.GetByIdAsync(request.MaintenanceRequest.MaintenanceRequestID);
            //if (maintenanceRequest == null) throw new MaintenanceRequestNotFoundException();

            //maintenanceRequest.RequestType = request.MaintenanceRequest.RequestType;
            //maintenanceRequest.SubmissionDate = request.MaintenanceRequest.SubmissionDate;
            //maintenanceRequest.RequestStatus = request.MaintenanceRequest.RequestStatus;
            //maintenanceRequest.Attachments = request.MaintenanceRequest.Attachments;
            //maintenanceRequest.MaintenanceType = request.MaintenanceRequest.MaintenanceType;
            //maintenanceRequest.Location = request.MaintenanceRequest.Location;
            //maintenanceRequest.EstimatedCost = request.MaintenanceRequest.EstimatedCost;
            //maintenanceRequest.ExpectedStartDate = request.MaintenanceRequest.ExpectedStartDate;
            //maintenanceRequest.ExpectedEndDate = request.MaintenanceRequest.ExpectedEndDate;

            //await _maintenanceRequestRepository.UpdateAsync(maintenanceRequest);
            //return Unit.Value;

            return Response.Updated();

        }
    }
}