using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Delete
{
    public class DeleteRequestHandler : IRequestHandler<DeleteRequestCommand, EndowmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteRequestCommand command, CancellationToken cancellationToken)
        {
            var request = await _unitOfWork.Requests.GetByIdAsync(command.Id);
            if (request == null)
            {
                return Response.FailureResponse("Id", "Request not found.");
            }

            await _unitOfWork.Requests.RemoveAsync(request);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}