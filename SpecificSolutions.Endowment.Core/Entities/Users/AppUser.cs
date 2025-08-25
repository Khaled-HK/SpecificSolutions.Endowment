namespace SpecificSolutions.Endowment.Core.Entities.Users
{
    public class AppUser //: IApplicationUser
    {
        public string Id { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public List<string> Permissions { get; private set; } = new List<string>();
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;

        // Private constructor for EF Core
        private AppUser() { }

        // Factory method for creating a new AppUser
        public static AppUser Create(string id, string name, string email, string firstName, string lastName, List<string> permissions)
        {
            return new AppUser
            {
                Id = id,
                Name = name,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Permissions = permissions ?? new List<string>()
            };
        }

        // Update method for updating AppUser properties
        public void Update(string name, string email, string firstName, string lastName, List<string> permissions)
        {
            Name = name;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Permissions = permissions ?? new List<string>();
        }

        // Method to add permission
        public void AddPermission(string permission)
        {
            if (!Permissions.Contains(permission))
            {
                Permissions.Add(permission);
            }
        }

        // Method to remove permission
        public void RemovePermission(string permission)
        {
            Permissions.Remove(permission);
        }
    }
}