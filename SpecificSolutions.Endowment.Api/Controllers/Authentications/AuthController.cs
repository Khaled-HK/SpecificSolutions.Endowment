using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ChangePassword;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ConfirmEmail;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ForgotPassword;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.RefreshToken;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ResetPassword;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Application.Models.Identity;

namespace SpecificSolutions.Endowment.Api.Controllers.Authentications
{
    [AllowAnonymous]
    public class AuthController : ApiController
    {
        private readonly IAuthenticator _authenticator;
        private readonly IMediator _mediator;

        public AuthController(IAuthenticator authenticator, IMediator mediator)
        {
            _authenticator = authenticator;
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<EndowmentResponse<IUserLogin>> Login(LoginCommand command, CancellationToken cancellationToken) =>
            await _mediator.Send(command, cancellationToken);

        [HttpPost("logout")]
        public async void Logout(CancellationToken cancellationToken = default)
        {
            await _authenticator.LogoutAsync();
        }

        [HttpPost("register")]
        public async Task<EndowmentResponse> Register(RegisterCommand command, CancellationToken cancellationToken) =>
            await _mediator.Send(command, cancellationToken);

        [HttpPost("refresh-token")]
        public async Task<EndowmentResponse<RefreshTokenResponse>> RefreshToken(RefreshTokenCommand command, CancellationToken cancellationToken) =>
            await _mediator.Send(command, cancellationToken);

        [HttpPost("forgot-password")]
        public async Task<EndowmentResponse> ForgotPassword(ForgotPasswordCommand command, CancellationToken cancellationToken) =>
            await _mediator.Send(command, cancellationToken);

        [HttpPost("reset-password")]
        public async Task<EndowmentResponse> ResetPassword(ResetPasswordCommand command, CancellationToken cancellationToken) =>
            await _mediator.Send(command, cancellationToken);

        [HttpPost("change-password")]
        public async Task<EndowmentResponse> ChangePassword(ChangePasswordCommand command, CancellationToken cancellationToken) =>
            await _mediator.Send(command, cancellationToken);

        [HttpPost("confirm-email")]
        public async Task<EndowmentResponse> ConfirmEmail(ConfirmEmailCommand command, CancellationToken cancellationToken) =>
            await _mediator.Send(command, cancellationToken);
    }
}