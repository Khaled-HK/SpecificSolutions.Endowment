namespace SpecificSolutions.Endowment.Application.Abstractions.Contracts
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string passwordHash);
    }
}
