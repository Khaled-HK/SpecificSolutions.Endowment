using SpecificSolutions.Endowment.Core.Entities.Banks;
using SpecificSolutions.Endowment.Core.Entities.ExpenditureChangeRequests;

namespace SpecificSolutions.Endowment.Core.Entities.Branchs
{
    public class Branch
    {
        public Guid Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;

        public Guid BankId { get; private set; } // Foreign key to Bank
        public Bank Bank { get; private set; } = null!;

        private HashSet<ExpenditureChangeRequest> _newExpenditureRequests = new();
        public IReadOnlyCollection<ExpenditureChangeRequest> NewExpenditureRequests => _newExpenditureRequests;

        private HashSet<ExpenditureChangeRequest> _currentExpenditureRequests = new();
        public IReadOnlyCollection<ExpenditureChangeRequest> CurrentExpenditureRequests => _currentExpenditureRequests;

        // Private constructor for EF Core
        private Branch() { }

        // Factory method for creating a new Branch
        public static Branch Create(string name, string address, string contactNumber, Guid bankId)
        {
            return new Branch
            {
                Name = name,
                Address = address,
                ContactNumber = contactNumber,
                BankId = bankId
            };
        }
    }
}