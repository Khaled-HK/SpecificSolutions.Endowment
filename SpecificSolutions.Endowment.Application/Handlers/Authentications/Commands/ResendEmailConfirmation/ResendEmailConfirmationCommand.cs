using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ResendEmailConfirmation
{
    public record ResendEmailConfirmationCommand : ICommand
    {
        public string Email { get; set; }
    }
}
