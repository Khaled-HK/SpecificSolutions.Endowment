using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register
{
    public class RegisterHandler : ICommandHandler<RegisterCommand>
    {
        private readonly IAuthenticator _authenticator;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterHandler(IAuthenticator authenticator,
                            ICurrentUser currentUser,
                            IUnitOfWork unitOfWork)
        {
            _authenticator = authenticator;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var user = await _authenticator.Register(command);
            if (user == null)
            {
                return Response.FailureResponse<IUserLogin>("The specified account could not be located. Please verify the account ID and try again.");
            }

            return Response.Responsee(user);
        }
    }
}
