using SpecificSolutions.Endowment.Core.Entities.Regions;

namespace SpecificSolutions.Endowment.Core.Entities.Cities
{
    public class City
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Country { get; private set; }

        // navigation property with Regions entity (one-to-many relationship)
        private HashSet<Region> _regions = new();
        public IReadOnlyCollection<Region> Regions => _regions;

        // Private constructor for EF Core
        private City() { }

        // Factory method for creating a new City
        public static City Create(string name, string country)
        {
            return new City
            {
                Name = name,
                Country = country
            };
        }

        // Seed method to create a new City
        public static City Seed(Guid id, string name, string country)
        {
            var city = new City
            {
                Id = id,
                Name = name,
                Country = country
            };
            return city;
        }
    }
}