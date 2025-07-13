using SpecificSolutions.Endowment.Core.Entities.Buildings;
using SpecificSolutions.Endowment.Core.Entities.Cities;
using SpecificSolutions.Endowment.Core.Entities.Offices;

namespace SpecificSolutions.Endowment.Core.Entities.Regions
{
    public class Region
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Country { get; private set; }

        // navigation property with city
        public Guid CityId { get; private set; }
        public City City { get; private set; }

        private HashSet<Building> _buildings = new();
        public IReadOnlyCollection<Building> Buildings => _buildings;

        // office navigation property
        private HashSet<Office> _offices = new();
        public IReadOnlyCollection<Office> Offices => _offices;

        // Private constructor for EF Core
        private Region() { }

        // Factory method for creating a new Region
        public static Region Create(string name, string country, Guid cityId)
        {
            return new Region
            {
                Name = name,
                Country = country,
                CityId = cityId,
            };
        }
        // Seed method to create a new Region
        public static Region Seed(Guid id, Guid cityId, string name, string country)
        {
            var region = new Region
            {
                Id = id,
                CityId = cityId,
                Name = name,
                Country = country
            };
            return region;
        }
    }
}