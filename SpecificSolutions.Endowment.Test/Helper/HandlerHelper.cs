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

        public async Task Handle<T>(T command, CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(command, cancellationToken);
        }
    }
}
