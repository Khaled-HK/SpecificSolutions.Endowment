
namespace SpecificSolutions.Endowment.Core.Models.FacilityDetails
{
    public interface ICreateFacilityDetailCommand
    {
        public int Quantity { get; set; }
        Guid ProductId { get; }
        Guid BuildingDetailId { get; set; }
    }
}
