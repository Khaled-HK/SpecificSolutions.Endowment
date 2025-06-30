using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Create
{
    public class CreateDemolitionRequestHandler : ICommandHandler<CreateDemolitionRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDemolitionRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateDemolitionRequestCommand request, CancellationToken cancellationToken)
        {
            //var DemolitionRequest = new DemolitionRequest
            //{
            //    RequestType = request.DemolitionRequest.RequestType,
            //    SubmissionDate = request.DemolitionRequest.SubmissionDate,
            //    RequestStatus = request.DemolitionRequest.RequestStatus,
            //    Attachments = request.DemolitionRequest.Attachments,
            //    Reason = request.DemolitionRequest.Reason,
            //    EstimatedCost = request.DemolitionRequest.EstimatedCost,
            //    EstimatedTime = request.DemolitionRequest.EstimatedTime
            //};

            //await _DemolitionRequestRepository.AddAsync(DemolitionRequest);
            //return DemolitionRequest.DemolitionRequestID;

            return Response.Added();
        }
    }
}