namespace SpecificSolutions.Endowment.Application.Models.DTOs.Users
{
    //TODO refactor and implement with defrence ath
    public interface IUserLogin
    {
        string Id { get; set; }
        string Name { get; set; }
        string Token { get; set; }
        List<string> Permissions { get; set; }
        string RefreshToken { get; set; }
    }

    public class UserLogin : IUserLogin
    {
        public UserLogin(string token = "", string? id = null, string name = "", string refreshToken = "", List<string> permissions = null)
        {
            Token = token;
            Id = id ?? Guid.NewGuid().ToString();
            Name = name;
            RefreshToken = refreshToken;
            Permissions = permissions ?? new List<string>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public List<string> Permissions { get; set; } = new List<string>();
    }
}