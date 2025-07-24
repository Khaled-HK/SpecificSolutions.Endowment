using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Update;
using SpecificSolutions.Endowment.Application.Validators.Authentications;
using SpecificSolutions.Endowment.Application.Validators.BuildingDetails;
using SpecificSolutions.Endowment.Application.Validators.Buildings;
using SpecificSolutions.Endowment.Application.Validators.Decisions;
using SpecificSolutions.Endowment.Application.Validators.Mosques;
using SpecificSolutions.Endowment.Application.Validators.Offices;
using SpecificSolutions.Endowment.Application.Validators.Cities;
using SpecificSolutions.Endowment.Application.Validators.Regions;
using SpecificSolutions.Endowment.Application.Validators.Requests;
using SpecificSolutions.Endowment.Application.Validators.Accounts;

namespace SpecificSolutions.Endowment.Application.Validators
{
    public static class ValidationContainer
    {
        public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
        {
            // Authentication Validators
            services.AddTransient<IValidator<RegisterCommand>, RegisterCommandValidator>();
            services.AddTransient<IValidator<LoginCommand>, LoginCommandValidator>();

            // Office Validators
            services.AddTransient<IValidator<CreateOfficeCommand>, CreateOfficeCommandValidator>();
            services.AddTransient<IValidator<UpdateOfficeCommand>, UpdateOfficeCommandValidator>();

            // Decision Validators
            services.AddTransient<IValidator<CreateDecisionCommand>, CreateDecisionCommandValidator>();
            services.AddTransient<IValidator<UpdateDecisionCommand>, UpdateDecisionCommandValidator>();

            // City Validators
            services.AddTransient<IValidator<CreateCityCommand>, CreateCityCommandValidator>();
            services.AddTransient<IValidator<UpdateCityCommand>, UpdateCityCommandValidator>();

            // Region Validators
            services.AddTransient<IValidator<CreateRegionCommand>, CreateRegionCommandValidator>();
            services.AddTransient<IValidator<UpdateRegionCommand>, UpdateRegionCommandValidator>();

            // Request Validators
            services.AddTransient<IValidator<CreateRequestCommand>, CreateRequestCommandValidator>();
            services.AddTransient<IValidator<UpdateRequestCommand>, UpdateRequestCommandValidator>();

            // Mosque Validators
            services.AddTransient<IValidator<CreateMosqueCommand>, CreateMosqueCommandValidator>();
            services.AddTransient<IValidator<UpdateMosqueCommand>, UpdateMosqueCommandValidator>();

            // Building Details Validators
            services.AddTransient<IValidator<CreateBuildingDetailCommand>, CreateBuildingDetailCommandValidator>();
            services.AddTransient<IValidator<UpdateBuildingDetailCommand>, UpdateBuildingDetailCommandValidator>();

            // Building Validators
            services.AddTransient<IValidator<CreateBuildingCommand>, CreateBuildingCommandValidator>();
            services.AddTransient<IValidator<UpdateBuildingCommand>, UpdateBuildingCommandValidator>();

            // Account Validators
            services.AddTransient<IValidator<CreateAccountCommand>, CreateAccountCommandValidator>();
            services.AddTransient<IValidator<UpdateAccountCommand>, UpdateAccountCommandValidator>();

            return services;
        }
    }
}
