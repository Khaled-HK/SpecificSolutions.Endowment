using SpecificSolutions.Endowment.Core.Entities.Regions;
using SpecificSolutions.Endowment.Core.Models.Cities;

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

        // Factory method using CreateCityCommand
        public static City Create(ICreateCityCommand command)
        {
            return new City
            {
                Name = command.Name,
                Country = command.Country
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

        // Update method using UpdateCityCommand
        public void Update(IUpdateCityCommand command)
        {
            Name = command.Name;
            Country = command.Country;
        }

        // Update method for updating city data
        public void Update(string name, string country)
        {
            Name = name;
            Country = country;
        }
    }
}