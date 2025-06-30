namespace SpecificSolutions.Endowment.Application.Models.DTOs.Email
{
    public class EmailSetting
    {
        public string ApiKey { get; set; } = string.Empty;
        public string FromAddress { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
    }
}
