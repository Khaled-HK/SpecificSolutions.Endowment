namespace SpecificSolutions.Endowment.Core.Models
{
    public interface IApplicationUser
    {
        string Name { get; set; }
        string Email { get; set; }
        List<string> Permissions { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        //ICollection<Decision> Decisions { get; set; }
        Guid Id { get; set; }
    }
}
