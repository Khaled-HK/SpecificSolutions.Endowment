using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
using Microsoft.Extensions.Logging; // Added for logging
using System.IdentityModel.Tokens.Jwt; // Added for JwtRegisteredClaimNames
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using Microsoft.Extensions.DependencyInjection; // Added for GetRequiredService

namespace SpecificSolutions.Endowment.Application.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserContextMiddleware> _logger; // Added logger field

        public UserContextMiddleware(RequestDelegate next, ILogger<UserContextMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            try
            {
                _logger.LogInformation("UserContextMiddleware: Starting to process request");

                // Get UserContext from service provider
                var userContext = serviceProvider.GetRequiredService<IUserContext>();

                // Check if user is authenticated
                if (context.User?.Identity?.IsAuthenticated == true)
                {
                    _logger.LogInformation("UserContextMiddleware: User is authenticated");
                    
                    // Try to get UserId from claims
                    var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ?? 
                                     context.User.FindFirst("UserId") ?? 
                                     context.User.FindFirst("sub");

                    if (userIdClaim != null && !string.IsNullOrEmpty(userIdClaim.Value))
                    {
                        _logger.LogInformation("UserContextMiddleware: Found UserId in claims: {UserId}", userIdClaim.Value);
                        userContext.SetUserId(userIdClaim.Value);
                    }
                    else
                    {
                        _logger.LogWarning("UserContextMiddleware: User is authenticated but no UserId found in claims");
                    }
                }
                else
                {
                    _logger.LogInformation("UserContextMiddleware: User is not authenticated, checking X-User-Id header");
                    
                    // For unauthenticated users, check for X-User-Id header
                    var userIdHeader = context.Request.Headers["X-User-Id"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(userIdHeader))
                    {
                        _logger.LogInformation("UserContextMiddleware: Found X-User-Id header: {UserId}", userIdHeader);
                        userContext.SetUserId(userIdHeader);
                    }
                    else
                    {
                        _logger.LogInformation("UserContextMiddleware: No X-User-Id header found for unauthenticated user");
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserContextMiddleware: Error occurred while processing request");
                await _next(context);
            }
        }
    }

    public static class UserContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserContextMiddleware>();
        }
    }
} 