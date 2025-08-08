using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ConfirmEmail
{
    public class ConfirmEmailHandler : ICommandHandler<ConfirmEmailCommand>
    {
        private readonly IAuthenticator _authenticator;

        public ConfirmEmailHandler(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public async Task<EndowmentResponse> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
        {
            var result = await _authenticator.ConfirmEmailAsync(command.Email, command.Token);
            return Response.Responsee(result);
        }
    }
} 