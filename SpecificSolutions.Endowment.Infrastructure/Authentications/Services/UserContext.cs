using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SpecificSolutions.Endowment.Infrastructure.Authentications.Services;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserContext> _logger;

    public UserContext(IHttpContextAccessor httpContextAccessor, ILogger<UserContext> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public Guid UserId
    {
        get
        {
            var userId = GetUserIdOrDefault();
            
            if (!userId.HasValue || userId.Value == Guid.Empty)
            {
                throw new ApplicationException("User context is unavailable - No UserId found");
            }
            
            return userId.Value;
        }
    }

    public Guid? GetUserIdOrDefault()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            _logger.LogWarning("UserContext: HttpContext is null");
            return null;
        }

        var userIdClaim = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            _logger.LogWarning("UserContext: No UserId claim found");
            return null;
        }

        if (Guid.TryParse(userIdClaim, out var userId))
        {
            _logger.LogInformation("UserContext: Retrieved UserId: {UserId}", userId);
            return userId;
        }

        _logger.LogWarning("UserContext: Could not parse UserId: {UserId}", userIdClaim);
        return null;
    }

    public bool HasUserId()
    {
        var userId = GetUserIdOrDefault();
        return userId.HasValue && userId.Value != Guid.Empty;
    }

    public void SetUserId(string userId)
    {
        // This is a simple implementation that logs the action
        // In a more complex scenario, you might want to store this in a scoped service or session
        _logger.LogInformation("UserContext: Setting UserId: {UserId}", userId);
        
        // Note: This is a simplified implementation
        // In a real application, you might want to store the userId in a scoped service
        // or use a different mechanism to maintain user context across the request
    }
} 