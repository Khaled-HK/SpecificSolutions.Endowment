namespace SpecificSolutions.Endowment.Core.Models.Cities
{
    public interface ICreateCityCommand
    {
        string Name { get; set; }
        string Country { get; set; }
    }
} 