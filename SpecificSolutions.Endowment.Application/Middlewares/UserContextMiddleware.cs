using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;

namespace SpecificSolutions.Endowment.Application.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;
        public UserContextMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, IUserContext userContext)
        {
            // حاول استخراج UserId من Claims (JWT)
            string? userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? context.User?.FindFirst("sub")?.Value
                              ?? context.User?.FindFirst("UserId")?.Value;

            // إذا لم يوجد توكن أو لم يوجد UserId في Claims، جرب X-User-Id (للحالات الخاصة فقط)
            if (string.IsNullOrEmpty(userId))
                userId = context.Request.Headers["X-User-Id"].FirstOrDefault();

            if (!string.IsNullOrEmpty(userId))
                userContext.SetUserId(userId);

            await _next(context);
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