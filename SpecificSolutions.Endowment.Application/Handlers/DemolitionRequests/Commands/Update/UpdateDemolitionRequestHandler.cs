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
            var demolitionRequest = await _unitOfWork.DemolitionRequests.GetByIdAsync(request.Id, cancellationToken);
            if (demolitionRequest == null)
            {
                return Response.FailureResponse("Id", "Demolition request not found.");
            }

            // تحديث خصائص DemolitionRequest
            demolitionRequest.UpdateDetails(
                location: request.Location,
                reason: request.Reason,
                estimatedCost: request.EstimatedCost,
                estimatedTime: request.EstimatedTime,
                contractorName: request.ContractorName
            );

            await _unitOfWork.DemolitionRequests.UpdateAsync(demolitionRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}