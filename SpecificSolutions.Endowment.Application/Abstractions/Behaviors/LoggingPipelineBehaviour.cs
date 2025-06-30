using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
//using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Abstractions.Behaviors
{
    public class LoggingPipelineBehaviour<TRequest, TResponse>(IUserContext currentUser) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            //var userId = currentUser.UserId;

            //logger.LogInformation("Request: {@RequestName} | UserId: {UserId} | RequestData: {@Request}",
            //    requestName, userId, request);

            var resilt = await next();

            //if (resilt.IsSuccess)
            //{
            //    logger.LogInformation("Response: {@Response}", resilt);
            //}
            //else
            //{
            //    logger.LogError("Response: {@Response}", resilt);
            //}

            return resilt;
        }
    }
}
