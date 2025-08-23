using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Delete
{
    [Authorize(Permissions = Permission.MaintenanceRequestDelete)]
    public class DeleteMaintenanceRequestCommand : ICommand
    {
        public Guid MaintenanceRequestID { get; set; }
    }
}