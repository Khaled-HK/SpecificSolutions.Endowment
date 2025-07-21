using MediatR;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Abstractions.Behaviors
{
    public class LoggingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand
        where TResponse : notnull
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;

        public LoggingPipelineBehaviour(
            IUserContext userContext,
            ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid();
            var userId = _userContext.GetUserIdOrDefault()?.ToString() ?? "Unauthenticated";

            _logger.LogInformation("Starting request {RequestName} | RequestId: {RequestId} | UserId: {UserId}",
                typeof(TRequest).Name, requestId, userId);

            var response = await next();

            if (response is EndowmentResponse endowmentResponse && !endowmentResponse.IsSuccess)
            {
                _logger.LogWarning("Request {RequestName} completed with warnings | RequestId: {RequestId} | UserId: {UserId} | Message: {Message}",
                    typeof(TRequest).Name, requestId, userId, endowmentResponse.Message);
            }
            else
            {
                _logger.LogInformation("Request {RequestName} completed successfully | RequestId: {RequestId} | UserId: {UserId}",
                    typeof(TRequest).Name, requestId, userId);
            }

            return response;
        }
    }
}
