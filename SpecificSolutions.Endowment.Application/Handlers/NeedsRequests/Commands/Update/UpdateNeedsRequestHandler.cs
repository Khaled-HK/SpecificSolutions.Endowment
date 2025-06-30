using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

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
            //var needsRequest = await _needsRequestRepository.GetByIdAsync(request.NeedsRequest.NeedsRequestID);
            //if (needsRequest == null) throw new NeedsRequestNotFoundException();

            //needsRequest.RequestType = request.NeedsRequest.RequestType;
            //needsRequest.SubmissionDate = request.NeedsRequest.SubmissionDate;
            //needsRequest.RequestStatus = request.NeedsRequest.RequestStatus;
            //needsRequest.Attachments = request.NeedsRequest.Attachments;
            //needsRequest.Description = request.NeedsRequest.Description;
            //needsRequest.Priority = request.NeedsRequest.Priority;

            //await _needsRequestRepository.UpdateAsync(needsRequest);
            //await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}