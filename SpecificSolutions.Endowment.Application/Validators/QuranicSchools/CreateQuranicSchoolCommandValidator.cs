using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.QuranicSchools
{
    public class CreateQuranicSchoolCommandValidator : AbstractValidator<CreateQuranicSchoolCommand>
    {
        public CreateQuranicSchoolCommandValidator()
        {
            //RuleFor(x => x.SchoolName).NotEmpty().MaximumLength(200);
            //RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
            //RuleFor(x => x.ContactInfo).NotEmpty().MaximumLength(100);
            //RuleFor(x => x.NumberOfStudents).GreaterThanOrEqualTo(0);
            //RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
        }
    }
}