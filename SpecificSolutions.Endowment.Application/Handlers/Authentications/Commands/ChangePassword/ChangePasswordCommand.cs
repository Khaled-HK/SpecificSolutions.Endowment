using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ChangePassword
{
    public record ChangePasswordCommand : ICommand
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
} 