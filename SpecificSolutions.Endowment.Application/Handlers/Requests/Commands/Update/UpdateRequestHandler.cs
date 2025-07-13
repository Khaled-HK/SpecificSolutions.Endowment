using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Exceptions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update
{//TODO use i handeler
    public class UpdateRequestHandler : IRequestHandler<UpdateRequestCommand, EndowmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateRequestCommand command, CancellationToken cancellationToken)
        {
            var request = await _unitOfWork.Requests.GetByIdAsync(command.Id);
            if (request == null)
            {
                throw new EntityNotFoundException(command.Id);
            }

            var decision = await _unitOfWork.Decisions.GetByIdAsync(command.DecisionId);
            if (decision == null)
            {
                throw new EntityNotFoundException(command.DecisionId);
            }

            request.UpdateRequest(command.Title, command.Description, command.ReferenceNumber, decisionId: decision.Id);

            _unitOfWork.Requests.Update(request);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}