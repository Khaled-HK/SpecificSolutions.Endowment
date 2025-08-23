using MediatR;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Abstractions.Behaviors
{
    public class LoggingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand
        where TResponse : notnull
    {
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;

        public LoggingPipelineBehaviour(
            ICurrentUser currentUser,
            ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
        {
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid();
            var userId = _currentUser.GetUserIdOrDefault()?.ToString() ?? "Unauthenticated";

            _logger.LogInformation("Starting request {RequestName} | RequestId: {RequestId} | UserId: {UserId}",
                typeof(TRequest).Name, requestId, userId);

            var response = await next();

            if (response is EndowmentResponse endowmentResponse && !endowmentResponse.IsSuccess)
            {
                _logger.LogWarning("Request {RequestName} failed | RequestId: {RequestId} | UserId: {UserId} | Error: {Error}",
                    typeof(TRequest).Name, requestId, userId, endowmentResponse.Message);
            }
            else
            {
                _logger.LogInformation("Completed request {RequestName} | RequestId: {RequestId} | UserId: {UserId}",
                    typeof(TRequest).Name, requestId, userId);
            }

            return response;
        }
    }
}
