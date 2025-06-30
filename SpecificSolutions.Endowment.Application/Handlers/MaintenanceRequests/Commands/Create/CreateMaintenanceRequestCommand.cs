using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Create
{
    public class CreateMaintenanceRequestCommand : CreateRequestCommand
    {
        public string MaintenanceType { get; set; }
        public double EstimatedCost { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
    }
}