using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.NeedsRequests;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Update
{
    public class UpdateNeedsRequestHandler : ICommandHandler<UpdateNeedsRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateNeedsRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateNeedsRequestCommand request, CancellationToken cancellationToken)
        {
            var needsRequest = await _unitOfWork.NeedsRequests.GetByIdAsync(request.Id);
            if (needsRequest == null)
            {
                return Response.FailureResponse("NeedsRequest not found");
            }

            // تحديث خصائص NeedsRequest
            needsRequest.UpdateDetails(
                needsType: request.NeedsType,
                location: request.Location,
                estimatedCost: (double)request.EstimatedCost,
                provider: request.Provider
            );

            await _unitOfWork.NeedsRequests.UpdateAsync(needsRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}