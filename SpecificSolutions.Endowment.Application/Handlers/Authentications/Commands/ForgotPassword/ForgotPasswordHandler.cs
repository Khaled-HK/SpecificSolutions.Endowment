using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ForgotPassword
{
    public class ForgotPasswordHandler : ICommandHandler<ForgotPasswordCommand>
    {
        private readonly IAuthenticator _authenticator;

        public ForgotPasswordHandler(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public async Task<EndowmentResponse> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            var result = await _authenticator.ForgotPasswordAsync(command.Email);
            return Response.Responsee(result);
        }
    }
} 