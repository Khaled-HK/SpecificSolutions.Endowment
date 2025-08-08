using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand : ICommand
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
} 