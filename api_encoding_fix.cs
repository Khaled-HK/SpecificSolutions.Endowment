// ================================================================================================
// إصلاح إعدادات الترميز العربي في API
// Arabic Encoding Fix for API Configuration
// ================================================================================================
// تضع هذا الكود في Program.cs أو في ApiContainer.cs حسب الحاجة

using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;

namespace SpecificSolutions.Endowment.Api
{
    public static class EncodingFix
    {
        /// <summary>
        /// إضافة هذه الطريقة إلى ApiContainer.cs في طريقة AddApiServices
        /// Add this method to ApiContainer.cs in the AddApiServices method
        /// </summary>
        public static IServiceCollection AddArabicEncodingSupport(this IServiceCollection services)
        {
            // إعداد JSON للتعامل مع العربية
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.PropertyNameCaseInsensitive = true;
                options.SerializerOptions.AllowTrailingCommas = true;
                options.SerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                // تمكين دعم Unicode للعربية
                options.SerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });

            // إعداد MVC/API للتعامل مع العربية
            services.AddControllers(options =>
            {
                // تأكد من UTF-8 في الاستجابات
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = false;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                // تمكين دعم Unicode للعربية - هذا مهم جداً!
                options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });

            return services;
        }

        /// <summary>
        /// إضافة هذه الطريقة إلى Program.cs بعد var app = builder.Build();
        /// Add this method to Program.cs after var app = builder.Build();
        /// </summary>
        public static WebApplication UseArabicEncodingMiddleware(this WebApplication app)
        {
            // إضافة middleware لتأكيد UTF-8 في جميع الاستجابات
            app.Use(async (context, next) =>
            {
                // تأكيد UTF-8 في Content-Type
                context.Response.OnStarting(() =>
                {
                    if (context.Response.ContentType != null && 
                        context.Response.ContentType.Contains("application/json") &&
                        !context.Response.ContentType.Contains("charset"))
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";
                    }
                    
                    // إضافة headers لدعم العربية
                    context.Response.Headers.Add("Accept-Charset", "utf-8");
                    return Task.CompletedTask;
                });

                await next();
            });

            return app;
        }
    }
}

// ================================================================================================
// كيفية الاستخدام - How to Use
// ================================================================================================

/* 
1. في ApiContainer.cs، غير الطريقة AddApiServices إلى:

public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
{
    // Register API controllers
    services.AddControllers();
    
    // إضافة دعم الترميز العربي - ADD THIS LINE
    services.AddArabicEncodingSupport();

    services.AddSingleton<IUserLogin, UserLogin>();
    // Register services from the Infrastructure layer 
    services.AddInfrastructureServices(configuration);
    //TODO use other concret if need
    services.AddScoped<IAuthenticator, Authenticator>();

    // Register services from the Application layer
    services.AddApplicationServices();

    // Register services from the Core layer
    services.AddCoreServices();
    // Register other API-specific services as needed

    return services;
}

2. في Program.cs، أضف بعد var app = builder.Build();

var app = builder.Build();

// إضافة دعم الترميز العربي - ADD THIS LINE
app.UseArabicEncodingMiddleware();

// باقي الكود...
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

*/

// ================================================================================================
// إعدادات إضافية للـ Connection String - Additional Connection String Settings
// ================================================================================================

/*
في appsettings.json، تأكد من أن Connection String يحتوي على:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=Swagger_Endowment22;Trusted_Connection=True;MultipleActiveResultSets=true;trustservercertificate=true;charset=utf8;Connection Timeout=30;"
  }
}

أو يمكن إضافة هذه الإعدادات:
- charset=utf8
- Connection Timeout=30
- Persist Security Info=False

*/

// ================================================================================================
// اختبار سريع - Quick Test
// ================================================================================================

/*
بعد تطبيق هذه التغييرات، يمكنك اختبار API endpoint:

GET /api/Office/filter

يجب أن تحصل على استجابة JSON مثل:

{
  "data": [
    {
      "office": "مكتب الأوقاف - طرابلس",
      "region": "منطقة الدهماني",
      "phoneNumber": "0218-84-1234579"
    }
  ]
}

بدلاً من:
{
  "data": [
    {
      "office": "ط…ظƒطھط¨ ط§ظ„ط£ظˆظ‚ط§ظپ - ط·ط±ط§ط¨ظ„ط³",
      "region": "ظ…ظ†ط·ظ‚ط© ط§ظ„ط¯ظ‡ظ…ط§ظ†ظٹ",
      "phoneNumber": "0218-84-1234579"
    }
  ]
}
*/