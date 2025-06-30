using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Update
{
    public class UpdateConstructionRequestHandler : ICommandHandler<UpdateConstructionRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateConstructionRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateConstructionRequestCommand request, CancellationToken cancellationToken)
        {
            //var constructionRequest = await _constructionRequestRepository.GetByIdAsync(request.ConstructionRequest.ConstructionRequestID);
            //if (constructionRequest == null) throw new ConstructionRequestNotFoundException();

            //constructionRequest.RequestType = request.ConstructionRequest.RequestType;
            //constructionRequest.SubmissionDate = request.ConstructionRequest.SubmissionDate;
            //constructionRequest.RequestStatus = request.ConstructionRequest.RequestStatus;
            //constructionRequest.Attachments = request.ConstructionRequest.Attachments;
            //constructionRequest.BuildingType = request.ConstructionRequest.BuildingType;
            //constructionRequest.ProposedLocation = request.ConstructionRequest.ProposedLocation;
            //constructionRequest.ProposedArea = request.ConstructionRequest.ProposedArea;
            //constructionRequest.EstimatedCost = request.ConstructionRequest.EstimatedCost;
            //constructionRequest.ContractorName = request.ConstructionRequest.ContractorName;

            //await _constructionRequestRepository.UpdateAsync(constructionRequest);
            //return Unit.Value;

            return Response.Updated();
        }
    }
}