using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SpecificSolutions.Endowment.Test.Helper
{
    public class HandlerHelper
    {
        private readonly IServiceProvider _provider;

        public HandlerHelper(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResult> Handle<T, TResult>(T command, CancellationToken cancellationToken) where T : IRequest<TResult>
        {
            using var scope = _provider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(command, cancellationToken);
        }
    }
}
