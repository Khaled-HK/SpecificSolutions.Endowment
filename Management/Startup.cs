using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpecificSolutions.Endowment.Management;
using SpecificSolutions.Endowment.Vue;
using VueCliMiddleware;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Dashboard
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment _env { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Handle XSRF Name for Header
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
            });

            services.AddApiServices(Configuration);
            services.AddCustomJwtAuth(Configuration);
            //services.AddDbContext<JeelContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CONINFO")));

            //services.Configure<Settings>(Configuration.GetSection("Settings"));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            var policy = new AuthorizationPolicyBuilder()
                      .RequireAuthenticatedUser()
                      .Build();

            // For refreshing view pages
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //services.AddDataProtection();

            services.AddMvc(config =>
            {
                config.Filters.Add(new AuthorizeFilter(policy));
                config.EnableEndpointRouting = false;

            });

            // Configure HTTPS redirection
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 5001; // Use the HTTPS port from launchSettings.json
            });

            //services.AddControllersWithViews(config =>
            //{  
            //});

            services.AddSpaStaticFiles(configuration =>
            {
                //development
                configuration.RootPath = "ClientApp";
                // publish
                configuration.RootPath = "ClientApp/dist";
            });

            //services.AddControllers(opt =>
            //    opt.Filters.Add(new AuthorizeFilter(policy))
            //    );

            // services.ConfigureApplicationCookie(options => options.LoginPath = "/LogIn");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationBuilder app2, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Only use HTTPS redirection in production or when HTTPS is available
            if (!env.IsDevelopment() || Configuration["ASPNETCORE_URLS"]?.Contains("https") == true)
            {
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSpaStaticFiles();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllerRoute(
                                name: "default",
                                pattern: "{controller}/{action=Index}/{id?}");
                            endpoints.MapRazorPages();
                        });

            // app.UseStatusCodePagesWithReExecute("/Login");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.Map("",
               adminApp =>
               {
                   app.UseSpa(spa =>
                   {
                       if (env.IsDevelopment())
                           spa.Options.SourcePath = "ClientApp";
                       else
                           spa.Options.SourcePath = "dist";
                       if (env.IsDevelopment())
                       {
                           spa.UseVueCli(npmScript: "serve");
                       }
                   });
               }
           );
        }
    }
}
