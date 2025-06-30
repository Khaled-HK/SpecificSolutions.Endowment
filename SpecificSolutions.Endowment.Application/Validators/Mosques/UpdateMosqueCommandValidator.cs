using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.Mosques
{
    public class UpdateMosqueCommandValidator : AbstractValidator<UpdateMosqueCommand>
    {
        public UpdateMosqueCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.FileNumber).NotEmpty().WithMessage("File Number is required");
            RuleFor(x => x.MosqueDefinition).NotEmpty().WithMessage("Mosque Definition is required");
            RuleFor(x => x.MosqueClassification).NotEmpty().WithMessage("Mosque Classification is required");
            RuleFor(x => x.OfficeId).NotEmpty().WithMessage("Office is required");
            RuleFor(x => x.Unit).NotEmpty().WithMessage("Unit is required");
            RuleFor(x => x.RegionId).NotEmpty().WithMessage("Region is required");
            RuleFor(x => x.NearestLandmark).NotEmpty().WithMessage("Nearest Landmark is required");
            RuleFor(x => x.ConstructionDate).NotEmpty().WithMessage("Construction Date is required");
            RuleFor(x => x.OpeningDate).NotEmpty().WithMessage("Opening Date is required");
            RuleFor(x => x.MapLocation).NotEmpty().WithMessage("Map Location is required");
            RuleFor(x => x.TotalLandArea).NotEmpty().WithMessage("Total Land Area is required");
            RuleFor(x => x.TotalCoveredArea).NotEmpty().WithMessage("Total Covered Area is required");
            RuleFor(x => x.NumberOfFloors).NotEmpty().WithMessage("Number Of Floors is required");
            RuleFor(x => x.ElectricityMeter).NotEmpty().WithMessage("Electricity Meter is required");
            RuleFor(x => x.AlternativeEnergySource).NotEmpty().WithMessage("Alternative Energy Source is required");
            RuleFor(x => x.WaterSource).NotEmpty().WithMessage("Water Source is required");
            RuleFor(x => x.Sanitation).NotEmpty().WithMessage("Sanitation is required");
            RuleFor(x => x.BriefDescription).NotEmpty().WithMessage("Brief Description is required");
        }
    }
}