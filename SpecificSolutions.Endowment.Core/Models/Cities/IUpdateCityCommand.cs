namespace SpecificSolutions.Endowment.Core.Models.Cities
{
    public interface IUpdateCityCommand
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Country { get; set; }
    }
} 