using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Exceptions;
using SpecificSolutions.Endowment.Application.Models.Global;
using System.Net;

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
                return false;
            }

            return true;
        }

        private async Task HandleValidationException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
        {
            var exception = (ValidationException)ex;

            var errors = exception.Errors
             .SelectMany(kvp => kvp.Value.Select(v => new Error(kvp.Key, v)))
             .ToArray();

            var response = new EndowmentResponse(state: ResponseState.BadRequest, "Validation failed", errors);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.Cookies.Append("Content-Type", "application/json");

            await httpContext.Response.WriteAsJsonAsync<EndowmentResponse>(response, cancellationToken);

        }

        private async Task<EndowmentResponse> HandleNotFoundException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
        {
            var exception = (NotFoundException)ex;

            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
            {
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception.Message
            });

            return await Task.FromResult(new EndowmentResponse(ResponseState.NotFound, exception.Message, null));
        }

        private async Task<EndowmentResponse> HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
            });

            return await Task.FromResult(new EndowmentResponse(ResponseState.Unauthorized, ex.Message, null));
        }

        private async Task<EndowmentResponse> HandleForbiddenAccessException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
            });

            return await Task.FromResult(new EndowmentResponse(ResponseState.Forbidden, ex.Message, null));
        }

        private async Task HandleConcurrencyException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
        {
            var exception = (DbUpdateConcurrencyException)ex;

            _logger.LogWarning(exception, "Concurrency conflict detected.");

            httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict; // 409 Conflict
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = (int)HttpStatusCode.Conflict,
                Title = "Concurrency conflict",
                Detail = "The data you are trying to update has been modified by another user. Please refresh and try again.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
            });
        }
    }
}