using SpecificSolutions.Endowment.Core.Entities.Branchs;

namespace SpecificSolutions.Endowment.Core.Entities.Banks
{
    public class Bank
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string ContactNumber { get; private set; }

        // navigation property branchs for one-to-many relationship 
        private HashSet<Branch> _branches = new();
        public IReadOnlyCollection<Branch> Branches => _branches;

        // Private constructor for EF Core
        private Bank() { }

        // Factory method for creating a new Bank
        public static Bank Create(string name, string address, string contactNumber)
        {
            return new Bank
            {
                Name = name,
                Address = address,
                ContactNumber = contactNumber
            };
        }
    }
}