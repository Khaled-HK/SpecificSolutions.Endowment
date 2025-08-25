using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ResetPassword
{
    public record ResetPasswordCommand : ICommand
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
} 