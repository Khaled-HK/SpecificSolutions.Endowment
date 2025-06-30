using Microsoft.AspNetCore.RateLimiting;
using SpecificSolutions.Endowment.Api;
using SpecificSolutions.Endowment.Application.Handlers;
using SpecificSolutions.Endowment.Application.Middlewares;

var CorsPolicyName = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);

// Register API services
builder.Services.AddApiServices(builder.Configuration);

// Add custom JWT authentication 
builder.Services.AddCustomJwtAuth(builder.Configuration);

// Register the global exception handler
//builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();

builder.Services.AddProblemDetails(option =>
{
    option.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method}{context.HttpContext.Request.Path}";
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Add rate limiting
builder.Services.AddRateLimiter(option =>
{
    option.AddFixedWindowLimiter("fixed", o =>
    {
        o.PermitLimit = 10;
        o.Window = TimeSpan.FromSeconds(5);
        o.QueueLimit = 0;
    });
    option.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// Register Swagger
builder.Services.RegisterSwagger();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
});

//$.ajax({
//type: "POST",
//    url: "/your-endpoint",
//    headers:
//    {
//        "X-XSRF-TOKEN": yourTokenValue
//    },
//    data: yourData,
//    success: function(response) {
//        Handle success
//    }
//});

//////builder.Services.Configure<CookiePolicyOptions>(options =>
//////{
//////    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
//////    options.CheckConsentNeeded = context => true;
//////    options.MinimumSameSitePolicy = SameSiteMode.None;
//////});

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, configurePolicy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

        configurePolicy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

//var policy = new AuthorizationPolicyBuilder()
//          .RequireAuthenticatedUser()
//          .Build();

//builder.Services.AddMvc(config =>
//{
//    config.Filters.Add(new AuthorizeFilter(policy));
//    config.EnableEndpointRouting = false;
//});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(CorsPolicyName);

//app.UseExceptionHandler();
app.UseExceptionHandler("/Home/Error");

app.UseHttpsRedirection();

// Use rate limiting
app.UseRateLimiter();

// Use JWT authentication
app.UseAuthentication();
app.UseAuthorization();

// Use Swagger documentation
app.UseSwaggerDocumentation(app.Environment);

app.UseMiddleware<ThreadCultureMiddleware>();

// Map the RequestController
app.MapControllers();

app.Run();

public partial class Program { }