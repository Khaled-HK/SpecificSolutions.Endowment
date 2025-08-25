using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register
{
    public record RegisterCommand : ICommand
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string OfficeId { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? ApprovedBy { get; set; }
    }
}
