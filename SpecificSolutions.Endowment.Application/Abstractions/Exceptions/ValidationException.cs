using FluentValidation.Results;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Abstractions.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        // ✅ Constructor جديد يدعم Error[]
        public ValidationException(params Error[] errors)
            : this()
        {
            Errors = errors
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        // ✅ Constructor جديد لحقل واحد
        public ValidationException(string propertyName, string errorMessage)
            : this(new Error(propertyName, errorMessage))
        {
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
