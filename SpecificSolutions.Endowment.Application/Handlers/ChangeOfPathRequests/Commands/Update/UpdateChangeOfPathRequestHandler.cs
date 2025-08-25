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
            var changeOfPathRequest = await _unitOfWork.ChangeOfPathRequests.GetByIdAsync(request.Id, cancellationToken);
            if (changeOfPathRequest == null)
            {
                return Response.FailureResponse("Id", "Change of path request not found.");
            }

            // تحديث خصائص ChangeOfPathRequest
            changeOfPathRequest.UpdateDetails(
                currentType: request.CurrentType,
                newType: request.NewType,
                reason: request.Reason
            );

            await _unitOfWork.ChangeOfPathRequests.UpdateAsync(changeOfPathRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}