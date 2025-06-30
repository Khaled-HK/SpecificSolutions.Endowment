using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Create
{
    public class CreateNeedsRequestHandler : ICommandHandler<CreateNeedsRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateNeedsRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateNeedsRequestCommand request, CancellationToken cancellationToken)
        {
            //var needsRequest = new NeedsRequest
            //{
            //    RequestType = request.NeedsRequest.RequestType,
            //    SubmissionDate = request.NeedsRequest.SubmissionDate,
            //    RequestStatus = request.NeedsRequest.RequestStatus,
            //    Attachments = request.NeedsRequest.Attachments,
            //    Description = request.NeedsRequest.Description,
            //    Priority = request.NeedsRequest.Priority
            //};

            //await _needsRequestRepository.AddAsync(needsRequest);
            //return needsRequest.NeedsRequestID;
            //await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}