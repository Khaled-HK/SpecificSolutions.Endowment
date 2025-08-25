using SpecificSolutions.Endowment.Core.Entities.FacilityDetails;
using SpecificSolutions.Endowment.Core.Models.Products;

namespace SpecificSolutions.Endowment.Core.Entities.Products
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        private HashSet<FacilityDetail> _FacilityDetails = new();
        public IReadOnlyCollection<FacilityDetail> FacilityDetails => _FacilityDetails;

        // Private constructor for EF Core
        private Product() { }

        // Factory method for creating a new Product
        public static Product Create(ICreateProductCommand command)
        {
            return new Product
            {
                Name = command.Name,
                Description = command.Description,
            };
        }

        public static Product Seed(Guid id, string name, string description)
        {
            return new Product
            {
                Id = id,
                Name = name,
                Description = description,
            };
        }

        // Update method for updating Product properties
        public void Update(IUpdateProductCommand command)
        {
            Name = command.Name;
            Description = command.Description;
        }

        // Update method for updating product data
        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}