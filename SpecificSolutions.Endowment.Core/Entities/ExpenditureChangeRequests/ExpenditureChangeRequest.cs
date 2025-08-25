using SpecificSolutions.Endowment.Core.Entities.Branchs;
using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Core.Entities.ExpenditureChangeRequests
{
    public class ExpenditureChangeRequest
    {
        public Guid Id { get; private set; }
        public Guid CurrentExpenditureBranchId { get; private set; }
        public Guid NewExpenditureBranchId { get; private set; }
        public Branch CurrentExpenditureBranch { get; private set; } = null!;
        public Branch NewExpenditureBranch { get; private set; } = null!;
        public string Reason { get; private set; } = string.Empty;
        public Guid RequestId { get; private set; }
        public Request Request { get; private set; } = null!;

        private ExpenditureChangeRequest() { }

        private ExpenditureChangeRequest(string currentExpenditure, string newExpenditure, string reason, Guid requestId, Request request)
        {
            Reason = reason;
            RequestId = requestId;
            Request = request;
        }

        public static ExpenditureChangeRequest Create(string currentExpenditure, string newExpenditure, string reason, Guid requestId, Request request)
        {
            return new ExpenditureChangeRequest(currentExpenditure, newExpenditure, reason, requestId, request);
        }
    }
}