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
        _logger.LogInformation($"ValidationPipelineBehavior: Processing request of type {typeof(TRequest).Name}");

        // إذا لم يكن الطلب command، تابع العملية بدون validation
        if (!(request is IBaseCommand))
        {
            _logger.LogInformation($"ValidationPipelineBehavior: Request is not IBaseCommand, skipping validation");
            return await next();
        }

        // إذا لم يوجد validators، تابع العملية
        if (!_validators.Any())
        {
            _logger.LogInformation($"ValidationPipelineBehavior: No validators found for {typeof(TRequest).Name}");
            return await next();
        }

        _logger.LogInformation($"ValidationPipelineBehavior: Found {_validators.Count()} validators for {typeof(TRequest).Name}");

        // تنفيذ الـ validation
        _logger.LogInformation($"ValidationPipelineBehavior: Running validation for {typeof(TRequest).Name}");
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            _logger.LogWarning($"ValidationPipelineBehavior: Validation failed with {failures.Count} errors");
            foreach (var failure in failures)
            {
                _logger.LogWarning($"Validation error: {failure.PropertyName} - {failure.ErrorMessage}");
            }

            var validationException = new SpecificSolutions.Endowment.Application.Abstractions.Exceptions.ValidationException(failures);
            _logger.LogWarning($"ValidationPipelineBehavior: Throwing ValidationException with {failures.Count} errors");
            throw validationException;
        }

        _logger.LogInformation($"ValidationPipelineBehavior: Validation passed for {typeof(TRequest).Name}");

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