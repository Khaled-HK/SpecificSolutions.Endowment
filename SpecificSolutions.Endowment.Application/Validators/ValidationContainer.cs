using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;
using SpecificSolutions.Endowment.Application.Validators.Authentications;
using SpecificSolutions.Endowment.Application.Validators.Offices;
using SpecificSolutions.Endowment.Application.Validators.Regions;
using SpecificSolutions.Endowment.Application.Validators.Requests;

namespace SpecificSolutions.Endowment.Application.Validators
{
    public static class ValidationContainer
    {
        public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CreateRequestCommand>, CreateRequestCommandValidator>();
            services.AddTransient<IValidator<RegisterCommand>, RegisterCommandValidator>();
            services.AddTransient<IValidator<LoginCommand>, LoginCommandValidator>();
            //services.AddTransient<IValidator<RefreshTokenCommand>, RefreshTokenCommandValidator>();
            services.AddTransient<IValidator<CreateOfficeCommand>, CreateOfficeCommandValidator>();
            services.AddTransient<IValidator<UpdateOfficeCommand>, UpdateOfficeCommandValidator>();
            services.AddTransient<IValidator<CreateDecisionCommand>, CreateDecisionCommandValidator>();
            services.AddTransient<IValidator<UpdateDecisionCommand>, UpdateDecisionCommandValidator>();
            services.AddTransient<IValidator<CreateRegionCommand>, CreateRegionCommandValidator>();
            services.AddTransient<IValidator<CreateRequestCommand>, CreateRequestCommandValidator>();
            services.AddTransient<IValidator<UpdateRequestCommand>, UpdateRequestCommandValidator>();

            return services;
        }
    }
}
