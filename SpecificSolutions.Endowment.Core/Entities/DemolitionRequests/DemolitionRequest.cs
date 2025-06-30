using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Core.Entities.DemolitionRequests
{
    public class DemolitionRequest
    {
        public Guid Id { get; private set; }
        public string Location { get; private set; }
        public string Reason { get; private set; }
        public double EstimatedReconstructionCost { get; private set; }
        public string ContractorName { get; private set; }

        public Guid RequestId { get; private set; }
        public Request Request { get; private set; }

        private DemolitionRequest() { }

        private DemolitionRequest(string location, string reason, double estimatedReconstructionCost, string contractorName, Guid requestId)
        {
            Location = location;
            Reason = reason;
            EstimatedReconstructionCost = estimatedReconstructionCost;
            ContractorName = contractorName;
            RequestId = requestId;
        }

        public static DemolitionRequest Create(string location, string reason, double estimatedReconstructionCost, string contractorName, Guid requestId)
        {
            return new DemolitionRequest(location, reason, estimatedReconstructionCost, contractorName, requestId);
        }

        public void UpdateDetails(string location, string reason, double estimatedReconstructionCost, string contractorName)
        {
            Location = location;
            Reason = reason;
            EstimatedReconstructionCost = estimatedReconstructionCost;
            ContractorName = contractorName;
        }
    }
}