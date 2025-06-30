using Microsoft.OpenApi.Models;
using System.Reflection;

namespace SpecificSolutions.Endowment.Api;

public static class AddSwaggerExtension
{
    public static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Endowment APIs",
                Version = "v1",
            });

            options.CustomSchemaIds(x =>
            {
                string value = $"{x.Namespace}.{x.Name}";

                while (x.GenericTypeArguments.Length > 0)
                {
                    x = x.GenericTypeArguments[0];
                    value += $"{x.Namespace}.{x.Name}";
                }

                value = value.Replace("SpecificSolutions.Endowment.RestAPI.Controllers.", "");

                return value;
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }

    public static void UseSwaggerDocumentation(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseSwagger(options =>
        {
            options.SerializeAsV2 = true;
        });

        app.UseSwaggerUI(c =>
        {
            if (environment.IsStaging() || environment.IsDevelopment() || environment.IsProduction())
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client API V1");
            }
        });
    }
}