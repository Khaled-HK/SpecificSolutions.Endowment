using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Delete
{
    public class DeleteDecisionHandler : IRequestHandler<DeleteDecisionCommand, EndowmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDecisionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteDecisionCommand command, CancellationToken cancellationToken)
        {
            var decision = await _unitOfWork.Decisions.GetByIdAsync(command.Id, cancellationToken);
            if (decision == null)
            {
                return Response.FailureResponse("Id", "Decision not found.");
            }

            await _unitOfWork.Decisions.RemoveAsync(decision);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}