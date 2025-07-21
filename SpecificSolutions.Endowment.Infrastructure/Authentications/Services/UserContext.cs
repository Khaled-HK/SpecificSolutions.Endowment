using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SpecificSolutions.Endowment.Infrastructure.Authentications.Services;

internal sealed class UserContext : IUserContext
{
    private Guid? _userId;
    public Guid UserId => _userId ?? throw new ApplicationException("User context is unavailable - No UserId found");
    public Guid? GetUserIdOrDefault() => _userId;
    public bool HasUserId() => _userId.HasValue && _userId.Value != Guid.Empty;
    public void SetUserId(string userId)
    {
        if (Guid.TryParse(userId, out var guid))
            _userId = guid;
    }
} 