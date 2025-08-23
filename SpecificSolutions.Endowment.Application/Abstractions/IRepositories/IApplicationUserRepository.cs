using SpecificSolutions.Endowment.Application.Models.DTOs.UserApprovals;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    /// <summary>
    /// Repository للمستخدمين - نمط خالد
    /// مسؤول فقط عن العمليات الأساسية (CRUD) على ApplicationUser
    /// </summary>
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        /// <summary>
        /// الحصول على مستخدم بواسطة ID (string)
        /// </summary>
        Task<ApplicationUser?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// الحصول على مستخدم بواسطة Email
        /// </summary>
        Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// الحصول على مستخدم بواسطة UserName
        /// </summary>
        Task<ApplicationUser?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default);

        /// <summary>
        /// التحقق من وجود مستخدم بواسطة Email
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// التحقق من وجود مستخدم بواسطة UserName
        /// </summary>
        Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default);

        /// <summary>
        /// الحصول على جميع المستخدمين مع Pagination
        /// </summary>
        Task<List<ApplicationUser>> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// الحصول على عدد جميع المستخدمين
        /// </summary>
        Task<int> GetAllUsersCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// الحصول على المستخدمين المعلقين مع Pagination - نمط خالد
        /// تنفيذ الفلترة والتحويل إلى DTO على مستوى قاعدة البيانات
        /// </summary>
        Task<List<PendingUserDto>> GetPendingUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// الحصول على عدد المستخدمين المعلقين - نمط خالد
        /// تنفيذ الفلترة على مستوى قاعدة البيانات
        /// </summary>
        Task<int> GetPendingUsersCountAsync(
            string? searchTerm = null,
            string? officeId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default);
    }
}
