using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Core.Entities.ChangeOfPathRequests
{
    public class ChangeOfPathRequest
    {
        public Guid Id { get; private set; }
        public string CurrentType { get; private set; } = string.Empty; // e.g., "Friday Mosque"
        public string NewType { get; private set; } = string.Empty; // e.g., "Regular Mosque"
        public string Reason { get; private set; } = string.Empty;
        public Guid RequestId { get; private set; }
        public Request Request { get; private set; } = null!;

        private ChangeOfPathRequest() { }

        private ChangeOfPathRequest(string currentType, string newType, string reason, Guid requestId, Request request)
        {
            CurrentType = currentType;
            NewType = newType;
            Reason = reason;
            RequestId = requestId;
            Request = request;
        }

        public static ChangeOfPathRequest Create(string currentType, string newType, string reason, Guid requestId, Request request)
        {
            return new ChangeOfPathRequest(currentType, newType, reason, requestId, request);
        }
    }
}