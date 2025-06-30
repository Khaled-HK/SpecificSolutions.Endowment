using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login
{
    public class LoginHandler : ICommandHandler<LoginCommand, IUserLogin>
    {
        private readonly IAuthenticator _authenticator;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        public LoginHandler(IAuthenticator authenticator,
                            ICurrentUser currentUser,
                            IUnitOfWork unitOfWork,
                            ITokenService tokenService)
        {
            _authenticator = authenticator;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<EndowmentResponse<IUserLogin>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _authenticator.LoginAsync(command);
            if (user == null)
            {
                return Response.FailureResponse<IUserLogin>("The specified account could not be located. Please verify the account ID and try again.");
            }

            var refreshToken = Models.Identity.Entities.RefreshToken.Create(user.Id, user.RefreshToken, DateTime.Now.AddHours(1));

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            //_currentUser.SetUser(user);
            _currentUser.UpdateUserInfo(user);

            return Response.SuccessLogin(user);
        }
    }
}
