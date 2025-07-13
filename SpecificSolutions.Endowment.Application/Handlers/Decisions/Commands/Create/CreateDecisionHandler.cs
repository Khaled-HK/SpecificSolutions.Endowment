using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Decisions;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Create
{
    public class CreateDecisionHandler : IRequestHandler<CreateDecisionCommand, EndowmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDecisionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateDecisionCommand command, CancellationToken cancellationToken)
        {
            var decision = new Decision(command);

            await _unitOfWork.Decisions.AddAsync(decision);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}