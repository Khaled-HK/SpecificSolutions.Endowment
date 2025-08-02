using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Requests
{
    public class CreateRequestCommandValidator : BaseValidator<CreateRequestCommand>
    {
        public CreateRequestCommandValidator()
        {
            RuleFor(command => command.Title)
                .NotEmpty().WithMessage(Messages.RequestTitleRequired)
                .Length(1, 100).WithMessage(Messages.RequestTitleLength)
                .Must(BeValidName).WithMessage(Messages.RequestTitleInvalidCharacters);

            RuleFor(command => command.Description)
                .NotEmpty().WithMessage(Messages.RequestDescriptionRequired)
                .Length(1, 500).WithMessage(Messages.RequestDescriptionLength)
                .Must(BeValidName).WithMessage(Messages.RequestDescriptionInvalidCharacters);

            RuleFor(command => command.CreatedDate)
                .NotEmpty().WithMessage(Messages.RequestCreatedDateRequired)
                .LessThanOrEqualTo(DateTime.Now).WithMessage(Messages.RequestCreatedDateNotInFuture);
        }
    }
}
