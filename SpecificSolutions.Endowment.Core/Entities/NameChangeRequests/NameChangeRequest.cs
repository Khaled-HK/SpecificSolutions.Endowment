using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Core.Entities.NameChangeRequests
{
    public class NameChangeRequest
    {
        public Guid Id { get; private set; }
        public string CurrentName { get; private set; }
        public string NewName { get; private set; }
        public string Reason { get; private set; }
        public Guid RequestId { get; private set; }
        public Request Request { get; private set; }

        private NameChangeRequest() { }

        private NameChangeRequest(string currentName, string newName, string reason, Guid requestId)
        {
            CurrentName = currentName;
            NewName = newName;
            Reason = reason;
            RequestId = requestId;
        }

        public static NameChangeRequest Create(string currentName, string newName, string reason, Guid requestId)
        {
            return new NameChangeRequest(currentName, newName, reason, requestId);
        }
    }
}