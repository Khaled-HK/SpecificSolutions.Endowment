using SpecificSolutions.Endowment.Core.Entities.BuildingDetails;
using SpecificSolutions.Endowment.Core.Entities.Products;
using SpecificSolutions.Endowment.Core.Models.FacilityDetails;

namespace SpecificSolutions.Endowment.Core.Entities.FacilityDetails
{
    public class FacilityDetail
    {
        public Guid Id { get; private set; }
        // quantity of the product
        public int Quantity { get; private set; }
        // price of the product 
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }

        public Guid BuildingDetailId { get; private set; }
        public BuildingDetail BuildingDetail { get; private set; }

        // Private constructor for EF Core
        private FacilityDetail() { }

        // Factory method for creating a new FacilityDetail
        public static FacilityDetail Create(ICreateFacilityDetailCommand command)
        {
            return new FacilityDetail
            {
                Quantity = command.Quantity,
                ProductId = command.ProductId,
            };
        }
    }
}