using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using System.Security.Claims;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
    string GenerateRefreshTokenAsync();
    string? GetUserIdFromToken(string token);
    Task<ClaimsPrincipal> GeneratePrincipalAsync(ApplicationUser user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}