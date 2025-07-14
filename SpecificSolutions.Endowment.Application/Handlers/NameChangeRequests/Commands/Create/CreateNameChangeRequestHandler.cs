using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.NameChangeRequests;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Create
{
    public class CreateNameChangeRequestHandler : ICommandHandler<CreateNameChangeRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateNameChangeRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateNameChangeRequestCommand request, CancellationToken cancellationToken)
        {
            // call static method to create a new NameChangeRequest
            var nameChangeRequest = NameChangeRequest.Create(request.CurrentName, request.NewName, request.Reason, request.Id);

            // add the new NameChangeRequest to the repository
            await _unitOfWork.NameChangeRequests.AddAsync(nameChangeRequest, cancellationToken);

            // save changes
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}