using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Core.Entities.DemolitionRequests
{
    public class DemolitionRequest
    {
        public Guid Id { get; private set; }
        public string Location { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public double EstimatedCost { get; set; }
        public int EstimatedTime { get; set; } // in days
        public string ContractorName { get; set; } = string.Empty;

        public Guid RequestId { get; private set; }
        public Request Request { get; private set; } = null!;

        private DemolitionRequest() { }

        private DemolitionRequest(string location, string reason, double estimatedCost, int estimatedTime, string contractorName, Guid requestId, Request request)
        {
            Location = location;
            Reason = reason;
            EstimatedCost = estimatedCost;
            EstimatedTime = estimatedTime;
            ContractorName = contractorName;
            RequestId = requestId;
            Request = request;
        }

        public static DemolitionRequest Create(string location, string reason, double estimatedCost, int estimatedTime, string contractorName, Guid requestId, Request request)
        {
            return new DemolitionRequest(location, reason, estimatedCost, estimatedTime, contractorName, requestId, request);
        }

        public void UpdateDetails(string location, string reason, double estimatedCost, int estimatedTime, string contractorName)
        {
            Location = location;
            Reason = reason;
            EstimatedCost = estimatedCost;
            EstimatedTime = estimatedTime;
            ContractorName = contractorName;
        }
    }
}