using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ResetPassword
{
    public class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand>
    {
        private readonly IAuthenticator _authenticator;

        public ResetPasswordHandler(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public async Task<EndowmentResponse> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var result = await _authenticator.ResetPasswordAsync(command.Email, command.Token, command.NewPassword);
            return Response.Responsee(result);
        }
    }
} 