namespace SpecificSolutions.Endowment.Core.Models.FacilityDetails
{
    public interface IUpdateFacilityDetailCommand
    {
        Guid Id { get; set; }
        Guid ProductId { get; set; }
        int Quantity { get; set; }
    }
}
