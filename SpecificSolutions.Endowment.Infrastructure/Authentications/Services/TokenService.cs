using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpecificSolutions.Endowment.Application.Models.Identity;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SpecificSolutions.Endowment.Infrastructure.Authentications.Services
{
    public class TokenService(IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings) : ITokenService
    {
        public async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var secretKey = jwtSettings.Value.Key;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync((ApplicationUser)user); // Fix to get roles
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new("sub", user.Id.ToString()),
                new(ClaimTypes.Email, user.Email)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Issuer = jwtSettings.Value.Issuer,
                Audience = jwtSettings.Value.Audience,
                //todo: change to 15 minutes
                Expires = DateTime.Now.AddYears((int)jwtSettings.Value.DurationInMinutes),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        public async Task<ClaimsPrincipal> GeneratePrincipalAsync(ApplicationUser user)
        {
            var secretKey = jwtSettings.Value.Key;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync((ApplicationUser)user); // Fix to get roles
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new("sub", user.Id.ToString()),
                new(ClaimTypes.Email, user.Email)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Issuer = jwtSettings.Value.Issuer,
                Audience = jwtSettings.Value.Audience,

                Expires = DateTime.Now.AddYears((int)jwtSettings.Value.DurationInMinutes),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        }

        public string? GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Value.Key);
            var validToken = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidIssuer = jwtSettings.Value.Issuer,
                ValidAudience = jwtSettings.Value.Audience,
                ValidateLifetime = false
            }, out SecurityToken validatedToken);

            if (validToken != null)
            {
                if (tokenHandler.ReadToken(token) is JwtSecurityToken securityToken)
                {
                    // حاول الحصول على NameIdentifier أو sub
                    var userIdClaim = securityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)
                        ?? securityToken.Claims.FirstOrDefault(claim => claim.Type == "sub");
                    return userIdClaim?.Value;
                }
            }

            return null;
        }

        public string GenerateRefreshTokenAsync()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Key)),
                ValidIssuer = jwtSettings.Value.Issuer,
                ValidAudience = jwtSettings.Value.Audience,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}