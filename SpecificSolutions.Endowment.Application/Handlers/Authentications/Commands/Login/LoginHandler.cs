using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Resources;
using System;

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
            try
            {
                var user = await _authenticator.LoginAsync(command);
                if (user == null)
                {
                    return Response.FailureResponse<IUserLogin>("", "بيانات الاعتماد غير صحيحة. يرجى التحقق من البريد الإلكتروني وكلمة المرور.");
                }

            var refreshToken = Models.Identity.Entities.RefreshToken.Create(user.Id, user.RefreshToken, DateTime.Now.AddHours(1));

            await _unitOfWork.RefreshTokens.AddAsync(refreshToken, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

                //_currentUser.SetUser(user);
                _currentUser.UpdateUserInfo(user);

                return Response.SuccessLogin(user);
            }
            catch (UnauthorizedAccessException)
            {
                return Response.FailureResponse<IUserLogin>("", "بيانات الاعتماد غير صحيحة. يرجى التحقق من البريد الإلكتروني وكلمة المرور.");
            }
            catch (Exception)
            {
                return Response.FailureResponse<IUserLogin>("", "حدث خطأ أثناء تسجيل الدخول. يرجى المحاولة مرة أخرى.");
            }
        }
    }
}
