using SpecificSolutions.Endowment.Core.Entities.BuildingDetailRequests;
using SpecificSolutions.Endowment.Core.Entities.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Core.Entities.ConstructionRequests;
using SpecificSolutions.Endowment.Core.Entities.Decisions;
using SpecificSolutions.Endowment.Core.Entities.DemolitionRequests;
using SpecificSolutions.Endowment.Core.Entities.EndowmentExpenditureChangeRequests;
using SpecificSolutions.Endowment.Core.Entities.MaintenanceRequests;
using SpecificSolutions.Endowment.Core.Entities.NameChangeRequests;
using SpecificSolutions.Endowment.Core.Entities.NeedsRequests;

namespace SpecificSolutions.Endowment.Core.Entities.Requests
{
    public class Request
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string ReferenceNumber { get; private set; }
        public string UserId { get; private set; }

        // Foreign key for Decision
        public Guid DecisionId { get; private set; }
        public Decision Decision { get; private set; } // Navigation property
                                                       // relationship with BuildingDetailRequests entity (one to many)

        private HashSet<BuildingDetailRequest> _buildingDetailRequests = new();
        public IReadOnlyCollection<BuildingDetailRequest> BuildingDetailRequests => _buildingDetailRequests;

        // Private constructor for EF Core
        private Request() { }

        // Public constructor for creating new instances
        public Request(string title, string description, string referenceNumber, Guid decisionId)
        {
            Title = title;
            Description = description;
            CreatedDate = DateTime.UtcNow;
            ReferenceNumber = referenceNumber;
            DecisionId = decisionId;
        }

        // Method to update the request
        public void UpdateRequest(string title, string description, string referenceNumber, Guid decisionId)
        {
            Title = title;
            Description = description;
            ReferenceNumber = referenceNumber;
            DecisionId = decisionId;
        }

        public ExpenditureChangeRequest ExpenditureChangeRequest { get; private set; }
        public ChangeOfPathRequest ChangeOfPathRequest { get; private set; }
        public ConstructionRequest ConstructionRequest { get; private set; }
        public DemolitionRequest DemolitionRequest { get; private set; }
        public MaintenanceRequest MaintenanceRequest { get; private set; }
        public NameChangeRequest NameChangeRequest { get; private set; }
        public NeedsRequest NeedsRequest { get; private set; }

    }
}