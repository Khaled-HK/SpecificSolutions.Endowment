using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Core.Models.Authentications;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login
{
    public class LoginCommand : ILoginCommand, ICommand<IUserLogin>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}