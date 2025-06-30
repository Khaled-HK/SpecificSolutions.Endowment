using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.QuranicSchools
{
    public class UpdateQuranicSchoolCommandValidator : AbstractValidator<UpdateQuranicSchoolCommand>
    {
        public UpdateQuranicSchoolCommandValidator()
        {
            RuleFor(x => x.SchoolName).NotEmpty().MaximumLength(200);
            // Other rules...
        }
    }
} 