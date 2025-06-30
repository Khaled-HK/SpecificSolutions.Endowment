namespace SpecificSolutions.Endowment.Application.Abstractions.Contracts
{
    public interface IDatabaseBuilder
    {
        Task InitializeAsync();
    }
}