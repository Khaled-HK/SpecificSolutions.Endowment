using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Delete
{
    public class DeleteMaintenanceRequestHandler : ICommandHandler<DeleteMaintenanceRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMaintenanceRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteMaintenanceRequestCommand request, CancellationToken cancellationToken)
        {
            return Response.Deleted();
        }
    }
}