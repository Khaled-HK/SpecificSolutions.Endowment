namespace SpecificSolutions.Endowment.Core.Models.Office
{
    public interface IUpdateOfficeCommand
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Location { get; set; }
        string PhoneNumber { get; set; }
    }
}
