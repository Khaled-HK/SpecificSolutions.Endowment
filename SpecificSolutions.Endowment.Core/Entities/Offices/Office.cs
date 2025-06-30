using SpecificSolutions.Endowment.Core.Entities.Buildings;
using SpecificSolutions.Endowment.Core.Entities.Regions;

namespace SpecificSolutions.Endowment.Core.Entities.Offices
{
    public sealed class Office
    {
        private Office() { }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid RegionId { get; private set; }
        public Region Region { get; private set; }
        public string PhoneNumber { get; private set; }
        public Guid UserId { get; private set; }

        private HashSet<Building> _buildings = new();
        public IReadOnlyCollection<Building> Buildings => _buildings;

        public Office(string name, string location, string phoneNumber, Guid regionId)
        {
            Id = Guid.NewGuid();
            Name = name;
            PhoneNumber = phoneNumber;
            RegionId = regionId;
        }

        // static factory method to create a new Office
        public static Office Create(string name, string location, string phoneNumber, Guid regionId)
        {
            return new Office(name, location, phoneNumber, regionId);
        }

        // seed method to create a new Office
        public static Office Seed(Guid id, Guid userId, string name, string location, string phoneNumber, Guid regionId)
        {
            var office = new Office(name, location, phoneNumber, regionId);
            office.Id = id;
            office.UserId = userId;
            return office;
        }

        public void Update(string name, string location, string phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }
    }
}