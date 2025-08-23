namespace SpecificSolutions.Endowment.Application.Models.DTOs.UserApprovals
{
    /// <summary>
    /// DTO للمستخدمين المعلقين (في انتظار الموافقة)
    /// </summary>
    public class PendingUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid OfficeId { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        
        /// <summary>
        /// الاسم الكامل للمستخدم
        /// </summary>
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
