using MediatR;
using Polly.Retry;
using SpecificSolutions.Endowment.Application.Helpers;


namespace SpecificSolutions.Endowment.Application.Abstractions.Behaviors
{
    public class RetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly AsyncRetryPolicy _retryPolicy;

        public RetryBehavior()
        {
            _retryPolicy = RetryPolicies.DbConcurrencyRetryPolicy;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            return await _retryPolicy.ExecuteAsync(async () => await next());
        }
    }
}
