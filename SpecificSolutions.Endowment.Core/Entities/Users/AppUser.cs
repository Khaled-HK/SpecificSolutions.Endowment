namespace SpecificSolutions.Endowment.Core.Entities.Users
{
    public class AppUser //: IApplicationUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}