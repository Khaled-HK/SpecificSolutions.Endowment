using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Update
{
    public class UpdateChangeOfPathRequestHandler : ICommandHandler<UpdateChangeOfPathRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateChangeOfPathRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateChangeOfPathRequestCommand request, CancellationToken cancellationToken)
        {
            //var changeOfPathRequest = await _changeOfPathRequestRepository.GetByIdAsync(request.ChangeOfPathRequest.ChangeOfPathRequestID);
            //if (changeOfPathRequest == null) throw new ChangeOfPathRequestNotFoundException();

            //changeOfPathRequest.RequestType = request.ChangeOfPathRequest.RequestType;
            //changeOfPathRequest.SubmissionDate = request.ChangeOfPathRequest.SubmissionDate;
            //changeOfPathRequest.RequestStatus = request.ChangeOfPathRequest.RequestStatus;
            //changeOfPathRequest.Attachments = request.ChangeOfPathRequest.Attachments;
            //changeOfPathRequest.CurrentType = request.ChangeOfPathRequest.CurrentType;
            //changeOfPathRequest.NewType = request.ChangeOfPathRequest.NewType;
            //changeOfPathRequest.Reason = request.ChangeOfPathRequest.Reason;

            //await _changeOfPathRequestRepository.UpdateAsync(changeOfPathRequest);
            //await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}