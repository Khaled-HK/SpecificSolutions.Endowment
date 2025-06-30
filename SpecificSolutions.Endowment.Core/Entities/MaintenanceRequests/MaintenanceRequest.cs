using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Core.Entities.MaintenanceRequests
{
    public class MaintenanceRequest
    {
        public Guid Id { get; private set; }
        public string MaintenanceType { get; private set; } // e.g., "Periodic", "Emergency"
        public string Location { get; private set; } // Building location needing maintenance
        public double EstimatedCost { get; private set; }
        public DateTime ExpectedStartDate { get; private set; }
        public DateTime ExpectedEndDate { get; private set; }
        public Guid RequestId { get; private set; }
        public Request Request { get; private set; }

        private MaintenanceRequest() { }

        public static MaintenanceRequest Create(string maintenanceType, string location, double estimatedCost, DateTime expectedStartDate, DateTime expectedEndDate, Guid requestId, Request request)
        {
            return new MaintenanceRequest
            {
                MaintenanceType = maintenanceType,
                Location = location,
                EstimatedCost = estimatedCost,
                ExpectedStartDate = expectedStartDate,
                ExpectedEndDate = expectedEndDate,
                RequestId = requestId,
                Request = request
            };
        }
    }
}