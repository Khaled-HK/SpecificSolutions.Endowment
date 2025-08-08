using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ForgotPassword
{
    public record ForgotPasswordCommand : ICommand
    {
        public string Email { get; set; }
    }
} 