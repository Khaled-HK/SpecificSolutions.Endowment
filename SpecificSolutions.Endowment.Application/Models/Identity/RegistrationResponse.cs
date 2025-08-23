namespace SpecificSolutions.Endowment.Application.Models.Identity
{
    public class RegistrationResponse
    {
        public string UserId { get; set; }
        public string? Message { get; set; }
        public bool RequiresApproval { get; set; } = true;
    }
}
