using Microsoft.AspNetCore.Identity;

namespace SpecificSolutions.Endowment.Application.Models.Identity.Entities
{
    public class RefreshToken : IdentityUserToken<string>
    {
        public RefreshToken() { }

        public string Token { get; private set; }
        public DateTime ExpiryDate { get; private set; }

        // Constructor for creating a new RefreshToken
        private RefreshToken(string userId, string token, DateTime expiryDate)
        {
            UserId = userId;
            Token = token;
            ExpiryDate = expiryDate;
            LoginProvider = "CustomProvider";
            //todo: check if this is correct
            Name = "JwtToken" + Guid.NewGuid();
        }

        //static method to create a new RefreshToken
        public static RefreshToken Create(string userId, string token, DateTime expiryDate)
        {
            return new RefreshToken(userId, token, expiryDate);
        }

        // Method to update the token
        public void UpdateToken(string token, DateTime expiryDate)
        {
            Token = token;
            ExpiryDate = expiryDate;
        }

        // Method to check if the token is expired
        public bool IsExpired()
        {
            return DateTime.UtcNow >= ExpiryDate;
        }
    }
}
