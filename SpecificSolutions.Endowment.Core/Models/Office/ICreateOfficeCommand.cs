namespace SpecificSolutions.Endowment.Core.Models.Office
{
    public interface ICreateOfficeCommand
    {
        string Name { get; set; }
        string Location { get; set; }
        string PhoneNumber { get; set; }
        Guid RegionId { get; set; }
    }
}
