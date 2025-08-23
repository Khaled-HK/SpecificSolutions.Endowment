using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.UserApprovals;

namespace SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Queries.GetPendingUsersCount
{
    /// <summary>
    /// Query للحصول على عدد المستخدمين المعلقين - نمط خالد
    /// يدعم الفلترة والتحويل إلى DTO على مستوى قاعدة البيانات
    /// </summary>
    public sealed record GetPendingUsersCountQuery : IQuery<int>
    {
        public GetPendingUsersCountQuery(
            string? searchTerm = null,
            string? officeId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            SearchTerm = searchTerm;
            OfficeId = officeId;
            FromDate = fromDate;
            ToDate = toDate;
        }

        /// <summary>
        /// مصطلح البحث في الاسم أو البريد الإلكتروني
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// معرف المكتب للفلترة
        /// </summary>
        public string? OfficeId { get; set; }

        /// <summary>
        /// تاريخ البداية للفلترة
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// تاريخ النهاية للفلترة
        /// </summary>
        public DateTime? ToDate { get; set; }

        // خصائص مطلوبة من IQuery حتى لو لم تُستخدم هنا
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
