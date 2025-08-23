using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register
{
    public class RegisterHandler : ICommandHandler<RegisterCommand>
    {
        private readonly IAuthenticator _authenticator;
        private readonly ICurrentUser _currentUser;

        public RegisterHandler(IAuthenticator authenticator,
                               ICurrentUser currentUser)
        {
            _authenticator = authenticator;
            _currentUser = currentUser;
        }

        public async Task<EndowmentResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var registrationResponse = await _authenticator.Register(command);
            if (registrationResponse == null)
            {
                return Response.FailureResponse<EndowmentResponse>("The registration failed. Please try again.");
            }

            return Response.Responsee(registrationResponse);
        }
    }
}
