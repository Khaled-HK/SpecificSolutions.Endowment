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
            var nameChangeRequest = await _unitOfWork.NameChangeRequests.GetByIdAsync(request.Id, cancellationToken);
            if (nameChangeRequest == null)
            {
                return Response.FailureResponse("Id", "Name change request not found.");
            }

            // تحديث خصائص NameChangeRequest
            nameChangeRequest.UpdateDetails(
                currentName: request.CurrentName,
                newName: request.NewName,
                reason: request.Reason
            );

            await _unitOfWork.NameChangeRequests.UpdateAsync(nameChangeRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}