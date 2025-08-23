using SpecificSolutions.Endowment.Application.Models.DTOs.UserApprovals;

namespace SpecificSolutions.Endowment.Application.Abstractions.Contracts
{
    /// <summary>
    /// عقد خدمة إدارة موافقات المستخدمين - نمط خالد
    /// </summary>
    public interface IUserApprovalService
    {
        /// <summary>
        /// الموافقة على مستخدم ومنحه صلاحيات
        /// </summary>
        Task<bool> ApproveUserAsync(string userId, string roleName, CancellationToken cancellationToken = default);

        /// <summary>
        /// رفض وحذف مستخدم
        /// </summary>
        Task<bool> RejectUserAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// التحقق من وجود مستخدم معلق
        /// </summary>
        Task<bool> IsPendingUserAsync(string userId, CancellationToken cancellationToken = default);
    }
}
