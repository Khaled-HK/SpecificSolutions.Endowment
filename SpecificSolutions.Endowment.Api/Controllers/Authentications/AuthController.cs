using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.RefreshToken;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register;
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
        public async Task<ActionResult<EndowmentResponse<IUserLogin>>> Login([FromBody] LoginCommand command1, CancellationToken cancellationToken)
        {
            var command = new LoginCommand
            {
                Email = "A@gmail.com",
                // Email = "employee@gmail.com",
                Password = "12345678"
            };

            var userLogin = await _mediator.Send(command, cancellationToken);
            return userLogin;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authenticator.LogoutAsync();
            return NoContent();
        }

        //[HttpPost("register")]
        //public async Task<ActionResult<RegistrationResponse>> Login(RegistrationRequest request)
        //{
        //    return Ok(await _authenticator.Register(request));
        //}

        [HttpPost("register")]
        public async Task<EndowmentResponse> Login(RegisterCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return response;
        }

        //Add refresh token endpoint
        [HttpPost("refresh-token")]
        public async Task<EndowmentResponse<RefreshTokenResponse>> RefreshToken(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return response;
        }


        // [HttpPost("forgot-password")]
        // public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword(ForgotPasswordRequest request)
        // {
        //     return Ok(await _authenticator.ForgotPassword(request));
        // }

        // [HttpPost("reset-password")]
        // public async Task<ActionResult<ResetPasswordResponse>> ResetPassword(ResetPasswordRequest request)
        // {
        //     return Ok(await _authenticator.ResetPassword(request));
        // }

        // [HttpPost("change-password")]
        // public async Task<ActionResult<ChangePasswordResponse>> ChangePassword(ChangePasswordRequest request)
        // {
        //     return Ok(await _authenticator.ChangePassword(request));
        // }

        // [HttpPost("confirm-email")]
        // public async Task<ActionResult<ConfirmEmailResponse>> ConfirmEmail(ConfirmEmailRequest request)
        // {
        //     return Ok(await _authenticator.ConfirmEmail(request));
        // }
    }
}