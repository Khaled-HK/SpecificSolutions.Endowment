using FluentValidation;

namespace SpecificSolutions.Endowment.Application.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected BaseValidator()
        {
            // Common validation rules can be added here

        }
    }
}
