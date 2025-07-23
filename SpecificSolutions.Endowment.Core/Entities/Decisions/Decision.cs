using SpecificSolutions.Endowment.Core.Entities.Requests;
using SpecificSolutions.Endowment.Core.Models.Decisions;

namespace SpecificSolutions.Endowment.Core.Entities.Decisions
{
    public class Decision
    {
        private Decision() { }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string ReferenceNumber { get; private set; }

        // Navigation property for related appuser
        public string UserId { get; private set; }
        //public AppUser AppUser { get; private set; }
        public ICollection<Request> Requests { get; private set; } = new List<Request>();

        // Constructor for creating a new Decision (آمن - UserId من JWT)
        public Decision(ICreateDecisionCommand command, string userId)
        {
            Title = command.Title;
            Description = command.Description;
            CreatedDate = command.CreatedDate != default ? command.CreatedDate : DateTime.UtcNow;
            ReferenceNumber = command.ReferenceNumber;
            UserId = userId; // من JWT token (آمن)
        }

        // Method to add a request to the decision
        public void AddRequest(Request request)
        {
            Requests.Add(request);
        }

        // Method to update the decision details (لا يتم تحديث UserId)
        public void Update(IUpdateDecisionCommand command)
        {
            Title = command.Title;
            Description = command.Description;
            ReferenceNumber = command.ReferenceNumber;
            // لا يتم تحديث UserId - يبقى كما هو لمنع انتحال الشخصية
        }
    }
}