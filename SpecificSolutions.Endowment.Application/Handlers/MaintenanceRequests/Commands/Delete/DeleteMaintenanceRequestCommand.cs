using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Delete
{
    public class DeleteMaintenanceRequestCommand : ICommand
    {
        public Guid MaintenanceRequestID { get; set; }
    }
}