using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SpecificSolutions.Endowment.Application.Models.Identity;
using System.Text;

namespace SpecificSolutions.Endowment.Api;

public static class JwtAuthExtensions
{
    public static IServiceCollection AddCustomJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuers = new[] { 
                    jwtSettings?.Issuer, 
                    "https://localhost:7128",  // Support old issuer for compatibility
                    "https://localhost:7141",  // Current HTTPS port
                    "http://localhost:7140"    // Current HTTP port
                },
                ValidAudiences = new[] { 
                    jwtSettings?.Audience,
                    "https://localhost:7104",  // Support old audience for compatibility  
                    "https://localhost:7141",  // Current HTTPS port
                    "http://localhost:7140"    // Current HTTP port
                },
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key ?? string.Empty))
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<object>>();
                    logger.LogWarning("JWT Authentication failed: {Error}", context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<object>>();
                    logger.LogInformation("JWT Token validated successfully");
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}