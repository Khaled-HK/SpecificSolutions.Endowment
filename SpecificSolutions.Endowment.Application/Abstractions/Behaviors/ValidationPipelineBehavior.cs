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
        var requestType = typeof(TRequest).Name;
        _logger.LogInformation("🔍 ValidationPipelineBehavior - معالجة طلب من نوع: {RequestType}", requestType);

        // إذا لم يكن الطلب command، تابع العملية بدون validation
        if (!(request is ICommand))
        {
            _logger.LogInformation("⏭️ الطلب ليس من نوع Command، تجاوز الـ validation");
            return await next();
        }

        // إذا لم يوجد validators، تابع العملية
        if (!_validators.Any())
        {
            _logger.LogInformation("⏭️ لا توجد validators لهذا النوع من الطلبات");
            return await next();
        }

        _logger.LogInformation("🔍 بدء عملية التحقق - عدد الـ validators: {ValidatorCount}", _validators.Count());

        // تنفيذ الـ validation
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        // إذا وُجدت أخطاء، ارمي ValidationException
        if (failures.Any())
        {
            _logger.LogWarning("❌ فشل التحقق - عدد الأخطاء: {ErrorCount}", failures.Count);
            foreach (var failure in failures)
            {
                _logger.LogWarning("❌ خطأ في {PropertyName}: {ErrorMessage}", failure.PropertyName, failure.ErrorMessage);
            }
            throw new SpecificSolutions.Endowment.Application.Abstractions.Exceptions.ValidationException(failures);
        }

        _logger.LogInformation("✅ التحقق نجح - متابعة إلى الـ handler");

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