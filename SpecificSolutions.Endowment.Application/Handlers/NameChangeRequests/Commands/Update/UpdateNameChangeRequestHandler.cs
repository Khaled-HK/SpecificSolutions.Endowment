using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Update
{
    public class UpdateNameChangeRequestHandler : ICommandHandler<UpdateNameChangeRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateNameChangeRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateNameChangeRequestCommand request, CancellationToken cancellationToken)
        {
            //var nameChangeRequest = await _nameChangeRequestRepository.GetByIdAsync(request.NameChangeRequest.NameChangeRequestID);
            //if (nameChangeRequest == null) throw new NameChangeRequestNotFoundException();

            //nameChangeRequest.RequestType = request.NameChangeRequest.RequestType;
            //nameChangeRequest.SubmissionDate = request.NameChangeRequest.SubmissionDate;
            //nameChangeRequest.RequestStatus = request.NameChangeRequest.RequestStatus;
            //nameChangeRequest.Attachments = request.NameChangeRequest.Attachments;
            //nameChangeRequest.CurrentName = request.NameChangeRequest.CurrentName;
            //nameChangeRequest.NewName = request.NameChangeRequest.NewName;
            //nameChangeRequest.Reason = request.NameChangeRequest.Reason;

            //await _nameChangeRequestRepository.UpdateAsync(nameChangeRequest);
            //await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}