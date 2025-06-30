using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Core.Entities.ConstructionRequests
{
    public class ConstructionRequest
    {
        public Guid Id { get; private set; }
        public string ProposedLocation { get; private set; }
        public double ProposedArea { get; private set; } // in square meters
        public double EstimatedCost { get; private set; }
        public string ContractorName { get; private set; }

        // relationships with Request entity one-to-one
        public Guid RequestId { get; private set; }
        public Request Request { get; private set; }

        private ConstructionRequest() { }

        private ConstructionRequest(string proposedLocation, double proposedArea, double estimatedCost, string contractorName, Guid requestId, Request request)
        {
            ProposedLocation = proposedLocation;
            ProposedArea = proposedArea;
            EstimatedCost = estimatedCost;
            ContractorName = contractorName;
            RequestId = requestId;
            Request = request;
        }

        public static ConstructionRequest Create(string proposedLocation, double proposedArea, double estimatedCost, string contractorName, Guid requestId, Request request)
        {
            return new ConstructionRequest(proposedLocation, proposedArea, estimatedCost, contractorName, requestId, request);
        }

        public void UpdateDetails(string proposedLocation, double proposedArea, double estimatedCost, string contractorName)
        {
            ProposedLocation = proposedLocation;
            ProposedArea = proposedArea;
            EstimatedCost = estimatedCost;
            ContractorName = contractorName;
        }
    }
}