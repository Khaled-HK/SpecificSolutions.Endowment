using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.Offices
{
    public class UpdateOfficeCommandValidator : AbstractValidator<UpdateOfficeCommand>
    {
        public UpdateOfficeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Office ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Office name is required.")
                .MaximumLength(100).WithMessage("Office name cannot exceed 100 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200).WithMessage("Location cannot exceed 200 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^(09[1-5]|02[1-9])-?\d{7}$").WithMessage("Phone number must be a valid Libyan format (e.g., 091-1234567, 021-1234567).");
        }
    }
}