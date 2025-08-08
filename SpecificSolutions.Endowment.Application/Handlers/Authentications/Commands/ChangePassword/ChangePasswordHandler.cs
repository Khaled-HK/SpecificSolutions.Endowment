using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ChangePassword
{
    public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand>
    {
        private readonly IAuthenticator _authenticator;

        public ChangePasswordHandler(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public async Task<EndowmentResponse> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var result = await _authenticator.ChangePasswordAsync(command.CurrentPassword, command.NewPassword);
            return Response.Responsee(result);
        }
    }
} 