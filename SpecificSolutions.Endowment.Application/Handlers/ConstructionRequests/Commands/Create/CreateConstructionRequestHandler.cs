using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Create
{
    public class CreateConstructionRequestHandler : ICommandHandler<CreateConstructionRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateConstructionRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateConstructionRequestCommand request, CancellationToken cancellationToken)
        {
            //var constructionRequest = new ConstructionRequest
            //{
            //    RequestType = request.ConstructionRequest.RequestType,
            //    SubmissionDate = request.ConstructionRequest.SubmissionDate,
            //    RequestStatus = request.ConstructionRequest.RequestStatus,
            //    Attachments = request.ConstructionRequest.Attachments,
            //    BuildingType = request.ConstructionRequest.BuildingType,
            //    ProposedLocation = request.ConstructionRequest.ProposedLocation,
            //    ProposedArea = request.ConstructionRequest.ProposedArea,
            //    EstimatedCost = request.ConstructionRequest.EstimatedCost,
            //    ContractorName = request.ConstructionRequest.ContractorName
            //};

            //await _constructionRequestRepository.AddAsync(constructionRequest);
            //await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}