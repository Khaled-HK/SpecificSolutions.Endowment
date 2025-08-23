using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SpecificSolutions.Endowment.Infrastructure.Authentications.Middleware
{
    /// <summary>
    /// Middleware موحد لتعيين ICurrentUser من JWT Token
    /// نمط خالد: Middleware نظيف ومتخصص يدير معلومات المستخدم الكاملة
    /// </summary>
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentUser currentUser)
        {
            try
            {
                // استخراج User ID من JWT Token
                var userId = GetUserIdFromToken(context);
                
                // إذا لم يوجد توكن أو لم يوجد UserId في Claims، جرب X-User-Id (للحالات الخاصة فقط)
                if (string.IsNullOrEmpty(userId))
                {
                    userId = context.Request.Headers["X-User-Id"].FirstOrDefault();
                }
                
                if (!string.IsNullOrEmpty(userId))
                {
                    // تعيين User ID في ICurrentUser
                    currentUser.SetUserId(userId);
                    
                    // استخراج UserName من Token أيضاً
                    var userName = GetUserNameFromToken(context);
                    if (!string.IsNullOrEmpty(userName))
                    {
                        currentUser.UserName = userName;
                    }
                }
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ ولكن لا نوقف الطلب
                Console.WriteLine($"خطأ في CurrentUserMiddleware: {ex.Message}");
            }

            await _next(context);
        }

        private string? GetUserIdFromToken(HttpContext context)
        {
            var user = context.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                // البحث عن User ID في Claims
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? 
                                 user.FindFirst("sub") ?? 
                                 user.FindFirst("userId") ?? 
                                 user.FindFirst("id");
                
                return userIdClaim?.Value;
            }
            
            return null;
        }

        private string? GetUserNameFromToken(HttpContext context)
        {
            var user = context.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                // البحث عن UserName في Claims
                var userNameClaim = user.FindFirst(ClaimTypes.Name) ?? 
                                   user.FindFirst("username") ?? 
                                   user.FindFirst("user_name");
                
                return userNameClaim?.Value;
            }
            
            return null;
        }
    }

    /// <summary>
    /// Extension method لتسهيل تسجيل Middleware
    /// </summary>
    public static class CurrentUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CurrentUserMiddleware>();
        }
    }
}
