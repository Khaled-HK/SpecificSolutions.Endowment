using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Core.Entities.NeedsRequests
{
    public class NeedsRequest
    {
        public Guid Id { get; private set; }
        public string NeedsType { get; private set; }
        public string Location { get; private set; } // Building location needing the items
        public double EstimatedCost { get; private set; }
        public string Provider { get; private set; } // Entity providing the needs
        public Guid RequestId { get; private set; }
        public Request Request { get; private set; }

        private NeedsRequest() { }

        private NeedsRequest(string needsType, string location, double estimatedCost, string provider, Guid requestId, Request request)
        {
            NeedsType = needsType;
            Location = location;
            EstimatedCost = estimatedCost;
            Provider = provider;
            RequestId = requestId;
            Request = request;
        }

        public static NeedsRequest Create(string needsType, string location, double estimatedCost, string provider, Guid requestId, Request request)
        {
            return new NeedsRequest(needsType, location, estimatedCost, provider, requestId, request);
        }

        public void UpdateDetails(string needsType, string location, double estimatedCost, string provider)
        {
            NeedsType = needsType;
            Location = location;
            EstimatedCost = estimatedCost;
            Provider = provider;
        }
    }
}