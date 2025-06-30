using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Update
{
    public class UpdateDemolitionRequestHandler : ICommandHandler<UpdateDemolitionRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDemolitionRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateDemolitionRequestCommand request, CancellationToken cancellationToken)
        {
            //var DemolitionRequest = await _DemolitionRequestRepository.GetByIdAsync(request.DemolitionRequest.DemolitionRequestID);
            //if (DemolitionRequest == null) throw new DemolitionRequestNotFoundException();

            //DemolitionRequest.RequestType = request.DemolitionRequest.RequestType;
            //DemolitionRequest.SubmissionDate = request.DemolitionRequest.SubmissionDate;
            //DemolitionRequest.RequestStatus = request.DemolitionRequest.RequestStatus;
            //DemolitionRequest.Attachments = request.DemolitionRequest.Attachments;
            //DemolitionRequest.Reason = request.DemolitionRequest.Reason;
            //DemolitionRequest.EstimatedCost = request.DemolitionRequest.EstimatedCost;
            //DemolitionRequest.EstimatedTime = request.DemolitionRequest.EstimatedTime;

            //await _DemolitionRequestRepository.UpdateAsync(DemolitionRequest);
            //return Unit.Value;

            return Response.Updated();
        }
    }
}