namespace SpecificSolutions.Endowment.Core.Models
{
    /// <summary>
    /// إعدادات التطبيق
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// الرابط الأساسي للتطبيق
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// رابط الواجهة الأمامية
        /// </summary>
        public string FrontendUrl { get; set; } = string.Empty;
    }
}
