using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Delete
{
    public class DeleteNameChangeRequestHandler : ICommandHandler<DeleteNameChangeRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNameChangeRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteNameChangeRequestCommand request, CancellationToken cancellationToken)
        {
            //var nameChangeRequest = await _nameChangeRequestRepository.GetByIdAsync(request.NameChangeRequestID);
            //if (nameChangeRequest == null) throw new NameChangeRequestNotFoundException();

            //await _nameChangeRequestRepository.DeleteAsync(request.NameChangeRequestID);
            //return Unit.Value;

            return Response.Deleted();
        }
    }
}