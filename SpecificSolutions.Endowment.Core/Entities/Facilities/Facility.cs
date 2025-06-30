namespace SpecificSolutions.Endowment.Core.Entities.Facilities
{
    public class Facility
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Location { get; private set; }
        public string ContactInfo { get; private set; }
        public int Capacity { get; private set; }
        public string Status { get; private set; }

        // Private constructor for EF Core
        private Facility() { }

        // Factory method for creating a new Facility
        public static Facility Create(string name, string location, string contactInfo, int capacity, string status)
        {
            return new Facility
            {
                Name = name,
                Location = location,
                ContactInfo = contactInfo,
                Capacity = capacity,
                Status = status
            };
        }
    }
}