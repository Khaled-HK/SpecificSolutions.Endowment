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
            var constructionRequest = await _unitOfWork.ConstructionRequests.GetByIdAsync(request.Id, cancellationToken);
            if (constructionRequest == null)
            {
                return Response.FailureResponse("Id", "Construction request not found.");
            }

            // تحديث خصائص ConstructionRequest
            constructionRequest.UpdateDetails(
                proposedLocation: request.ProposedLocation,
                proposedArea: request.ProposedArea,
                estimatedCost: request.EstimatedCost,
                contractorName: request.ContractorName
            );

            await _unitOfWork.ConstructionRequests.UpdateAsync(constructionRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}