using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create
{
    public class CreateRequestHandler : ICommandHandler<CreateRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateRequestCommand command, CancellationToken cancellationToken)
        {
            // Validate command
            if (string.IsNullOrWhiteSpace(command.Title))
            {
                return Response.FailureResponse("Title", "Title is required.");
            }

            // Check if ReferenceNumber exists
            if (await _unitOfWork.RequestRepository.ReferenceNumberExists(command.ReferenceNumber))
            {
                return Response.FailureResponse("ReferenceNumber", $"Request with ReferenceNumber `{command.ReferenceNumber}` already exists.");
            }
            //Get the decisionId from the decision table    
            // Get the decisionId from the decision table
            var decision = await _unitOfWork.DecisionRepository.GetByIdAsync(command.DecisionId);
            if (decision == null)
            {
                return Response.FailureResponse("DecisionId", "Invalid DecisionId.");
            }
            var decisionId = decision.Id;

            // Create the Request entity
            var request = new Request(command.Title, command.Description, command.ReferenceNumber, decisionId: decisionId);

            // Add the request to the repository
            await _unitOfWork.RequestRepository.AddAsync(request);

            // Commit the transaction
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Return success response
            return Response.Added();
        }
    }
}
