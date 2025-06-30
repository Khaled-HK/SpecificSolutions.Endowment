using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Delete
{
    public class DeleteDemolitionRequestHandler : ICommandHandler<DeleteDemolitionRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDemolitionRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteDemolitionRequestCommand request, CancellationToken cancellationToken)
        {
            //var DemolitionRequest = await _DemolitionRequestRepository.GetByIdAsync(request.DemolitionRequestID);
            //if (DemolitionRequest == null) throw new DemolitionRequestNotFoundException();

            //await _DemolitionRequestRepository.DeleteAsync(request.DemolitionRequestID);
            //await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}