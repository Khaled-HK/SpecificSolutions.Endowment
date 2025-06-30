using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Create
{
    public class CreateMaintenanceRequestHandler : ICommandHandler<CreateMaintenanceRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMaintenanceRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateMaintenanceRequestCommand request, CancellationToken cancellationToken)
        {
            //var maintenanceRequest = new MaintenanceRequest
            //{
            //    RequestType = request.MaintenanceRequest.RequestType,
            //    SubmissionDate = request.MaintenanceRequest.SubmissionDate,
            //    RequestStatus = request.MaintenanceRequest.RequestStatus,
            //    Attachments = request.MaintenanceRequest.Attachments,
            //    MaintenanceType = request.MaintenanceRequest.MaintenanceType,
            //    Location = request.MaintenanceRequest.Location,
            //    EstimatedCost = request.MaintenanceRequest.EstimatedCost,
            //    ExpectedStartDate = request.MaintenanceRequest.ExpectedStartDate,
            //    ExpectedEndDate = request.MaintenanceRequest.ExpectedEndDate
            //};

            //await _maintenanceRequestRepository.AddAsync(maintenanceRequest);
            //await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}