using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Application.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Vue.Common;

namespace SpecificSolutions.EndowmentVue.Controllers
{
    [Produces("application/json")]
    [Route("Security")]
    public class SecurityController : Controller
    {
        //private IConfiguration _configuration;
        //private readonly IAuthenticator<IUserLogin> _authenticator;
        private readonly IMediator _mediator;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public SecurityController(IMediator mediator, IOptions<JwtSettings> jwtSettings)
        {
            //_configuration = configuration;
            _mediator = mediator;
            //_authenticator = authenticator;
            _jwtSettings = jwtSettings;
        }

        [AllowAnonymous]
        [HttpGet("IsLoggedin")]
        public async Task<IActionResult> IsLogin(string returnUrl = null)
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                return Ok(true);
            }
            else
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok(false);
            }
        }



        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<EndowmentResponse<IUserLogin>>> loginUser([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var response = await _mediator.Send(command, cancellationToken);

                if (!response.IsSuccess)
                {
                    return StatusCode(statusCode: ((int)response.State), response);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, response.Data.Id),
                    new Claim(ClaimTypes.Name, response.Data.Name),
                    //new Claim(ClaimTypes.Email, model.Email),
                    new Claim("JwtToken", response.Data.Token)
                };

                if (response.Data.Permissions != null)
                {
                    claims.AddRange(response.Data.Permissions.Select(p => new Claim(ClaimTypes.Role, p)));
                }

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = command.RememberMe,
                    ExpiresUtc = command.RememberMe
                        ? DateTime.UtcNow.AddDays(30)
                        : DateTime.UtcNow.AddMinutes(_jwtSettings.Value.DurationInMinutes),
                    AllowRefresh = true,
                    //RedirectUri = returnUrl ?? Url.Content("~/")
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                    authProperties);

                return Ok(new { Id = response.Data.Id, UserName = response.Data.Name });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(BackMessages.StatusCode, BackMessages.LogoutError);
            }
        }
    }
}
