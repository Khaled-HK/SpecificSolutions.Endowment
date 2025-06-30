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
            //var needsRequest = await _needsRequestRepository.GetByIdAsync(request.NeedsRequestID);
            //if (needsRequest == null) throw new NeedsRequestNotFoundException();

            //await _needsRequestRepository.DeleteAsync(request.NeedsRequestID);
            //return Unit.Value;

            return Response.Deleted();
        }
    }
}