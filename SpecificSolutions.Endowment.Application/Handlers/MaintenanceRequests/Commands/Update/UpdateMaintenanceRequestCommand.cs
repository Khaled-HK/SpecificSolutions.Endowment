using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Update
{
    public class UpdateMaintenanceRequestCommand : UpdateRequestCommand
    {
        public string MaintenanceType { get; set; }
        public double EstimatedCost { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
    }
}