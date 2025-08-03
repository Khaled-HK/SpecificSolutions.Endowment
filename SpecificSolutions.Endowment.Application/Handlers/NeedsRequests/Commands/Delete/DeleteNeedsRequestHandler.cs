using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Delete
{
    public class DeleteNeedsRequestHandler : ICommandHandler<DeleteNeedsRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNeedsRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteNeedsRequestCommand request, CancellationToken cancellationToken)
        {
            var needsRequest = await _unitOfWork.NeedsRequests.GetByIdAsync(request.Id);
            if (needsRequest == null)
            {
                return Response.FailureResponse("NeedsRequest not found");
            }

            await _unitOfWork.NeedsRequests.DeleteAsync(request.Id);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}