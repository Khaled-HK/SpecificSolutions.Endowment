using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Infrastructure.Authentications.Services
{
    /// <summary>
    /// خدمة الصلاحيات - مسؤولة فقط عن إدارة الصلاحيات والأدوار
    /// </summary>
    public class PermissionService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISessionService _sessionService;

        public PermissionService(UserManager<ApplicationUser> userManager, ISessionService sessionService)
        {
            _userManager = userManager;
            _sessionService = sessionService;
        }

        /// <summary>
        /// الحصول على أدوار المستخدم
        /// </summary>
        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception($"User with ID {userId} not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        /// <summary>
        /// الحصول على صلاحيات المستخدم من ApplicationUserRole
        /// </summary>
        public async Task<List<string>> GetUserPermissionsAsync(ApplicationUser user)
        {
            try
            {
                var permissions = new List<string>();

                // الحصول على الصلاحيات من ApplicationUserRole entities
                var userPermissions = await GetUserRolePermissionsAsync(user.Id);
                permissions.AddRange(userPermissions);

                // إزالة التكرار وإرجاع قائمة نظيفة
                return permissions.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get permissions for user {user.Id}", ex);
            }
        }

        /// <summary>
        /// التحقق من وجود المستخدم في دور معين
        /// </summary>
        public async Task<bool> IsUserInRoleAsync(string roleName)
        {
            var applicationUser = await _sessionService.GetUser();
            if (applicationUser == null)
            {
                return false;
            }

            return await _userManager.IsInRoleAsync(applicationUser, roleName);
        }

        /// <summary>
        /// الحصول على الصلاحيات المحددة من ApplicationUserRole entities
        /// 
        /// كل مجموعة دور لها صلاحيات محددة:
        /// - Admin Group: AccountView, AccountAdd, UserView, UserAdd, etc.
        /// - Customer Group: AccountView, RequestView, RequestAdd, etc.
        /// - Employee Group: RequestView, OfficeView, etc.
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>قائمة بنصوص الصلاحيات المحددة من كائنات Permission</returns>
        private async Task<List<string>> GetUserRolePermissionsAsync(string userId)
        {
            try
            {
                var permissions = new List<string>();

                // تحميل المستخدم مع UserRoles والصلاحيات المحددة
                var userWithRoles = await _userManager.Users
                    .Where(u => u.Id == userId)
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .Select(u => new
                    {
                        UserRoles = u.UserRoles.Select(ur => new
                        {
                            ur.Permissions,
                            RoleName = ur.Role.Name, // اسم المجموعة: "Admin", "Customer", إلخ.
                            RoleId = ur.Role.Id
                        })
                    })
                    .FirstOrDefaultAsync();

                // استخراج الصلاحيات المحددة من كل مجموعة دور
                if (userWithRoles?.UserRoles != null)
                {
                    foreach (var userRole in userWithRoles.UserRoles)
                    {
                        if (userRole.Permissions != null)
                        {
                            // الحصول على الصلاحيات المفصلة لهذه المجموعة
                            var permissionList = userRole.Permissions.ToPermissionList();
                            
                            // إضافة الصلاحيات بدون سياق للاستخدام العام
                            permissions.AddRange(permissionList);
                        }
                    }
                }
                
                return permissions;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get role permissions for user {userId}", ex);
            }
        }
    }
}
