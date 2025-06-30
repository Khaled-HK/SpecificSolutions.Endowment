using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Delete
{
    public class DeleteChangeOfPathRequestHandler : ICommandHandler<DeleteChangeOfPathRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteChangeOfPathRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteChangeOfPathRequestCommand request, CancellationToken cancellationToken)
        {
            //var changeOfPathRequest = await _changeOfPathRequestRepository.GetByIdAsync(request.ChangeOfPathRequestID);
            //if (changeOfPathRequest == null) throw new ChangeOfPathRequestNotFoundException();

            //await _changeOfPathRequestRepository.DeleteAsync(request.ChangeOfPathRequestID);
            //await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}