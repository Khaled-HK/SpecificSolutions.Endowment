using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Delete
{
    public class DeleteExpenditureChangeRequestHandler : ICommandHandler<DeleteExpenditureChangeRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteExpenditureChangeRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteExpenditureChangeRequestCommand request, CancellationToken cancellationToken)
        {
            return Response.Deleted();

        }
    }
}