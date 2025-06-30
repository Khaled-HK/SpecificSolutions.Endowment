namespace SpecificSolutions.Endowment.Core.Models.Authentications
{
    public interface ILoginCommand
    {
        string Password { get; set; }
        string Email { get; set; }
    }
}
