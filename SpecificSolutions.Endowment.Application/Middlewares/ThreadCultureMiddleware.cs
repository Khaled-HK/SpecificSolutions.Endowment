using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace SpecificSolutions.Endowment.Application.Middlewares
{
    public class ThreadCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public ThreadCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // if ThreadCultureMiddleware regestration from Program.cs  get the language from the header if form wpf get the language from the current 
            // thread culture
            //var language = Thread.CurrentThread.CurrentCulture.Name;
            //var language = "en-US";

            var language = context.Request.Headers["Accept-Language"].ToString();

            //if language is not provided in the header, set the default language from current thread culture
            if (string.IsNullOrEmpty(language))
            {
                language = Thread.CurrentThread.CurrentCulture.Name;
                var culture = new CultureInfo(language);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }
            else
            {
                var headerLanguage = language.Split(',').FirstOrDefault();

                //var headerLanguage = "ar-LY";

                if (!string.IsNullOrEmpty(headerLanguage))
                {
                    var culture = new CultureInfo(headerLanguage);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                }
                else
                {
                    var culture = new CultureInfo("ar-LY");
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                }
            }

            await _next(context);
        }
    }
}
