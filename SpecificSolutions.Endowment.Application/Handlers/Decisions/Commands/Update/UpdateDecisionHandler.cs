using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Update
{
    public class UpdateDecisionHandler : IRequestHandler<UpdateDecisionCommand, EndowmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDecisionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateDecisionCommand command, CancellationToken cancellationToken)
        {
            var decision = await _unitOfWork.DecisionRepository.GetByIdAsync(command.Id);
            if (decision == null)
            {
                return Response.FailureResponse("Id", "Decision not found.");
            }

            decision.Update(command);

            _unitOfWork.DecisionRepository.Update(decision);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}