using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.Accounts
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
        }
    }
}