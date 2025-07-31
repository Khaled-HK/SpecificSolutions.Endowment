using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

public class ValidationPipelineBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : EndowmentResponse
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationPipelineBehavior<TRequest, TResponse>> _logger;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationPipelineBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // إذا لم يكن الطلب command، تابع العملية بدون validation
        if (!(request is ICommand))
        {
            return await next();
        }

        // إذا لم يوجد validators، تابع العملية
        if (!_validators.Any())
        {
            return await next();
        }

        // تنفيذ الـ validation
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        //var errors = _validators
        //    .Select(validator => validator.Validate(request))
        //    .SelectMany(validationResult => validationResult.Errors)
        //    .Where(validationFailure => validationFailure != null)
        //    .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage))
        //    .Distinct()
        //    .ToArray();
        if (failures.Any())
        {
            throw new SpecificSolutions.Endowment.Application.Abstractions.Exceptions.ValidationException(failures);
        }

        // تابع العملية إذا لم توجد أخطاء
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