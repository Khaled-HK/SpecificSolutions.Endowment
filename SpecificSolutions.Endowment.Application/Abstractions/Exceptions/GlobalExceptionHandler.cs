using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Exceptions;
using SpecificSolutions.Endowment.Application.Models.Global;
using System.Net;
using ValidationException = SpecificSolutions.Endowment.Application.Abstractions.Exceptions.ValidationException;

namespace SpecificSolutions.Endowment.Application.Handlers
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly Dictionary<Type, Func<HttpContext, Exception, CancellationToken, Task>> _exceptionHandlers;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
            _exceptionHandlers = new()
            {
                { typeof(ValidationException), HandleValidationException},
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
                { typeof(DbUpdateConcurrencyException), HandleConcurrencyException } // Add concurrency handler
            };
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            //var errorCode = exception switch
            //{
            //    EntityNotFoundException => (int)HttpStatusCode.BadRequest + "01",
            //    ValidationException => (int)HttpStatusCode.BadRequest + "02",
            //    _ => (int)HttpStatusCode.InternalServerError + "00"
            //};

            var exceptionType = exception.GetType();

            if (_exceptionHandlers.ContainsKey(exceptionType))
            {
                await _exceptionHandlers[exceptionType].Invoke(httpContext, exception, cancellationToken);
                return true; // إصلاح: يجب إرجاع true عندما يتم التعامل مع الـ exception
            }

            return false; // إرجاع false فقط عندما لا يتم التعامل مع الـ exception
        }

        private async Task HandleValidationException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
        {
            var exception = (ValidationException)ex;

            _logger.LogWarning(exception, "Validation errors occurred.");

            // تحويل Dictionary<string, string[]> إلى Errors
            var errors = exception.Errors
                .SelectMany(kvp => kvp.Value.Select(errorMessage => new Error(kvp.Key, errorMessage)))
                .ToArray();

            _logger.LogInformation($"Validation errors: {errors.Length} errors found");
            foreach (var error in errors)
            {
                _logger.LogInformation($"Validation error: {error.PropertyName} - {error.ErrorMessage}");
            }

            var response = new EndowmentResponse(state: ResponseState.BadRequest, "Validation failed", errors);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync<EndowmentResponse>(response, cancellationToken);
        }

        private async Task HandleNotFoundException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
        {
            var exception = (NotFoundException)ex;

            _logger.LogWarning(exception, "Resource not found: {Message}", exception.Message);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            var response = new EndowmentResponse(ResponseState.NotFound, exception.Message, null);
            await httpContext.Response.WriteAsJsonAsync<EndowmentResponse>(response, cancellationToken);
        }

        private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
        {
            _logger.LogWarning(ex, "Unauthorized access attempt.");

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var response = new EndowmentResponse(ResponseState.Unauthorized, ex.Message, null);
            await httpContext.Response.WriteAsJsonAsync<EndowmentResponse>(response, cancellationToken);
        }

        private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
        {
            _logger.LogWarning(ex, "Forbidden access attempt.");

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            var response = new EndowmentResponse(ResponseState.Forbidden, ex.Message, null);
            await httpContext.Response.WriteAsJsonAsync<EndowmentResponse>(response, cancellationToken);
        }

        private async Task HandleConcurrencyException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
        {
            var exception = (DbUpdateConcurrencyException)ex;

            _logger.LogWarning(exception, "Concurrency conflict detected.");

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict; // 409 Conflict

            var response = new EndowmentResponse(ResponseState.BadRequest, "The data you are trying to update has been modified by another user. Please refresh and try again.", null);
            await httpContext.Response.WriteAsJsonAsync<EndowmentResponse>(response, cancellationToken);
        }
    }
}