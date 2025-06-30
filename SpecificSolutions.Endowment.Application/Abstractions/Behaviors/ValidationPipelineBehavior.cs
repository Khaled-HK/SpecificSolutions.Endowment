using FluentValidation;
using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

public class ValidationPipelineBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : class //IRequest// notnull // IRequest<EndowmentResponse<TResponse>>
    //where TResponse : notnull// EndowmentResponse, new()

     where TResponse : EndowmentResponse
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) =>
        _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        //if TRequest is not a command, then return next

        if (!(request is ICommand))
        {
            return await next();
        }

        if (!_validators.Any())
        {
            return await next();
        }

        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            var errors = _validators
                .Select(validator => validator.Validate(request))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure != null)
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage))
                .Distinct()
                .ToArray();


            if (failures.Any())
                throw new SpecificSolutions.Endowment.Application.Abstractions.Exceptions.ValidationException(failures);
        }

        return await next();

        //var errors = _validators
        //    .Select(validator => validator.Validate(request))
        //    .SelectMany(validationResult => validationResult.Errors)
        //    .Where(validationFailure => validationFailure != null)
        //    .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage))
        //    .Distinct()
        //    .ToArray();

        //if (errors.Any())
        //{
        //    return CreateValidationResult<TResponse>(errors);
        //}

        //return await next();
    }

    private static T CreateValidationResult<T>(Error[] errors) where T : EndowmentResponse, new()
    {
        var validationResult = new T
        {
            Errors = errors,
        };

        return validationResult;
    }
}