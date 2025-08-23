using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.DTOs.UserApprovals;
using SpecificSolutions.Endowment.Application.Models.Identity;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using SpecificSolutions.Endowment.Infrastructure.Persistence;

namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// خدمة إدارة موافقات المستخدمين - للمسؤولين فقط
    /// نمط خالد: فصل منطق الأعمال عن Repository
    /// </summary>
    public class UserApprovalService : IUserApprovalService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _dbContext;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<UserApprovalService> _logger;

        public UserApprovalService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            AppDbContext dbContext,
            ICurrentUser currentUser,
            ILogger<UserApprovalService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        /// <summary>
        /// الحصول على المستخدمين المعلقين مع Pagination - نمط خالد
        /// تنفيذ الفلترة والتحويل إلى DTO على مستوى قاعدة البيانات
        /// </summary>
        public async Task<List<PendingUserDto>> GetPendingUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        { 
            var users = await _dbContext.Users
                .Where(u => !u.IsApproved)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new PendingUserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserName = u.UserName,
                    PhoneNumber = u.PhoneNumber,
                    OfficeId = u.OfficeId,
                    CreatedDate = u.CreatedDate
                })
                .ToListAsync(cancellationToken);
            
            return users;
        }

        /// <summary>
        /// الحصول على عدد المستخدمين المعلقين
        /// </summary>
        public async Task<int> GetPendingUsersCountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Where(u => !u.IsApproved)
                .CountAsync(cancellationToken);
        }

        /// <summary>
        /// الموافقة على مستخدم ومنحه صلاحيات - نمط خالد
        /// </summary>
        public async Task<bool> ApproveUserAsync(string userId, string roleName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("UserId cannot be null or empty", nameof(userId));
                
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentException("RoleName cannot be null or empty", nameof(roleName));

            _logger.LogInformation("Approving user {UserId} with role {RoleName}", userId, roleName);
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsUserApproved())
            {
                _logger.LogWarning("User {UserId} not found or already approved", userId);
                return false;
            }

            var currentUserId = _currentUser.GetUserIdOrDefault();
            if (!currentUserId.HasValue)
                throw new UnauthorizedAccessException("يجب تسجيل الدخول لتنفيذ هذه العملية.");

            // الموافقة على المستخدم
            user.ApproveUser(currentUserId.Value.ToString());
            _logger.LogInformation("User {UserId} approved successfully", userId);

            // إضافة الدور والصلاحيات
            await AddUserToRoleWithPermissionsAsync(user, roleName, cancellationToken);

            // حفظ التغييرات
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("User {UserId} approval completed successfully", userId);
            
            return true;
        }

        /// <summary>
        /// رفض وحذف مستخدم - نمط خالد
        /// </summary>
        public async Task<bool> RejectUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("UserId cannot be null or empty", nameof(userId));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsUserApproved())
                return false;

            // حذف المستخدم
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("User {UserId} rejected and deleted successfully", userId);
            }
            else
            {
                _logger.LogError("Failed to delete user {UserId}: {Errors}", userId, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            
            return result.Succeeded;
        }

        /// <summary>
        /// التحقق من وجود مستخدم معلق
        /// </summary>
        public async Task<bool> IsPendingUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            return await _dbContext.Users
                .Where(u => u.Id == userId && !u.IsApproved)
                .AnyAsync(cancellationToken);
        }

        /// <summary>
        /// إضافة المستخدم للدور مع الصلاحيات المناسبة
        /// </summary>
        private async Task AddUserToRoleWithPermissionsAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new InvalidOperationException($"الدور {roleName} غير موجود");

            // الحصول على الصلاحيات المناسبة للدور
            var permissions = GetDefaultPermissionsForRole(roleName);
            if (permissions == null)
                throw new InvalidOperationException($"فشل في الحصول على الصلاحيات للدور {roleName}");

            // التحقق من وجود السجل مسبقاً
            var existingUserRole = await _dbContext.ApplicationUserRole
                .FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, cancellationToken);

            if (existingUserRole != null)
            {
                _logger.LogInformation("Updating existing user role permissions for user {UserId} and role {RoleId}", user.Id, role.Id);
                
                // تحديث الصلاحيات الموجودة
                existingUserRole.Permissions = permissions;
                _dbContext.ApplicationUserRole.Update(existingUserRole);
            }
            else
            {
                _logger.LogInformation("Creating new user role for user {UserId} and role {RoleId}", user.Id, role.Id);
                
                // إنشاء ApplicationUserRole مع الصلاحيات المناسبة للدور
                var userRole = ApplicationUserRole.Create(
                    userId: user.Id,
                    roleId: role.Id,
                    permissions: permissions
                );

                // إضافة العلاقة إلى قاعدة البيانات مباشرة (بدون استخدام AddToRoleAsync)
                await _dbContext.ApplicationUserRole.AddAsync(userRole, cancellationToken);
            }
        }

        /// <summary>
        /// الحصول على الصلاحيات الافتراضية لكل دور
        /// </summary>
        private Permission GetDefaultPermissionsForRole(string roleName)
        {
            return roleName.ToLower() switch
            {
                "admin" => Permission.Seed(), // جميع الصلاحيات
                "customer" => Permission.Create(
                    // Account permissions
                    accountView: true, accountAdd: true, accountEdit: true, accountDelete: true,
                    // AccountDetail permissions
                    accountDetailView: true, accountDetailAdd: false, accountDetailEdit: false, accountDetailDelete: false,
                    // User permissions
                    userView: false, userAdd: false, userEdit: false, userDelete: false,
                    // Role permissions
                    roleView: false, roleAdd: false, roleEdit: false, roleDelete: false,
                    // Decision permissions
                    decisionView: true, decisionAdd: false, decisionEdit: false, decisionDelete: false,
                    // Request permissions
                    requestView: true, requestAdd: true, requestEdit: true, requestDelete: true,
                    // ConstructionRequest permissions
                    constructionRequestView: true, constructionRequestAdd: true, constructionRequestEdit: true, constructionRequestDelete: false,
                    // MaintenanceRequest permissions
                    maintenanceRequestView: true, maintenanceRequestAdd: true, maintenanceRequestEdit: true, maintenanceRequestDelete: false,
                    // DemolitionRequest permissions
                    demolitionRequestView: true, demolitionRequestAdd: true, demolitionRequestEdit: true, demolitionRequestDelete: false,
                    // NameChangeRequest permissions
                    nameChangeRequestView: true, nameChangeRequestAdd: true, nameChangeRequestEdit: true, nameChangeRequestDelete: false,
                    // NeedsRequest permissions
                    needsRequestView: true, needsRequestAdd: true, needsRequestEdit: true, needsRequestDelete: false,
                    // ExpenditureChangeRequest permissions
                    expenditureChangeRequestView: true, expenditureChangeRequestAdd: true, expenditureChangeRequestEdit: true, expenditureChangeRequestDelete: false,
                    // ChangeOfPathRequest permissions
                    changeOfPathRequestView: true, changeOfPathRequestAdd: true, changeOfPathRequestEdit: true, changeOfPathRequestDelete: false,
                    // Office permissions
                    officeView: true, officeAdd: false, officeEdit: false, officeDelete: false,
                    // Endowment permissions
                    endowmentView: true, endowmentAdd: false, endowmentEdit: false, endowmentDelete: false,
                    // City permissions
                    cityView: true, cityAdd: false, cityEdit: false, cityDelete: false,
                    // Region permissions
                    regionView: true, regionAdd: false, regionEdit: false, regionDelete: false,
                    // Building permissions
                    buildingView: true, buildingAdd: false, buildingEdit: false, buildingDelete: false,
                    // Mosque permissions
                    mosqueView: true, mosqueAdd: false, mosqueEdit: false, mosqueDelete: false,
                    // Product permissions
                    productView: true, productAdd: false, productEdit: false, productDelete: false,
                    // Bank permissions
                    bankView: true, bankAdd: false, bankEdit: false, bankDelete: false,
                    // Branch permissions
                    branchView: true, branchAdd: false, branchEdit: false, branchDelete: false,
                    // Facility permissions
                    facilityView: true, facilityAdd: false, facilityEdit: false, facilityDelete: false,
                    // BuildingDetailRequest permissions
                    buildingDetailRequestView: true, buildingDetailRequestAdd: false, buildingDetailRequestEdit: false, buildingDetailRequestDelete: false,
                    // QuranicSchool permissions
                    quranicSchoolView: true, quranicSchoolAdd: false, quranicSchoolEdit: false, quranicSchoolDelete: false
                ),
                "employee" => Permission.Create(
                    // Account permissions
                    accountView: true, accountAdd: true, accountEdit: true, accountDelete: false,
                    // AccountDetail permissions
                    accountDetailView: true, accountDetailAdd: true, accountDetailEdit: true, accountDetailDelete: false,
                    // User permissions
                    userView: false, userAdd: false, userEdit: false, userDelete: false,
                    // Role permissions
                    roleView: false, roleAdd: false, roleEdit: false, roleDelete: false,
                    // Decision permissions
                    decisionView: true, decisionAdd: false, decisionEdit: false, decisionDelete: false,
                    // Request permissions
                    requestView: true, requestAdd: true, requestEdit: true, requestDelete: false,
                    // ConstructionRequest permissions
                    constructionRequestView: true, constructionRequestAdd: true, constructionRequestEdit: true, constructionRequestDelete: false,
                    // MaintenanceRequest permissions
                    maintenanceRequestView: true, maintenanceRequestAdd: true, maintenanceRequestEdit: true, maintenanceRequestDelete: false,
                    // DemolitionRequest permissions
                    demolitionRequestView: true, demolitionRequestAdd: true, demolitionRequestEdit: true, demolitionRequestDelete: false,
                    // NameChangeRequest permissions
                    nameChangeRequestView: true, nameChangeRequestAdd: true, nameChangeRequestEdit: true, nameChangeRequestDelete: false,
                    // NeedsRequest permissions
                    needsRequestView: true, needsRequestAdd: true, needsRequestEdit: true, needsRequestDelete: false,
                    // ExpenditureChangeRequest permissions
                    expenditureChangeRequestView: true, expenditureChangeRequestAdd: true, expenditureChangeRequestEdit: true, expenditureChangeRequestDelete: false,
                    // ChangeOfPathRequest permissions
                    changeOfPathRequestView: true, changeOfPathRequestAdd: true, changeOfPathRequestEdit: true, changeOfPathRequestDelete: false,
                    // Office permissions
                    officeView: true, officeAdd: false, officeEdit: false, officeDelete: false,
                    // Endowment permissions
                    endowmentView: true, endowmentAdd: false, endowmentEdit: false, endowmentDelete: false,
                    // City permissions
                    cityView: true, cityAdd: false, cityEdit: false, cityDelete: false,
                    // Region permissions
                    regionView: true, regionAdd: false, regionEdit: false, regionDelete: false,
                    // Building permissions
                    buildingView: true, buildingAdd: false, buildingEdit: false, buildingDelete: false,
                    // Mosque permissions
                    mosqueView: true, mosqueAdd: false, mosqueEdit: false, mosqueDelete: false,
                    // Product permissions
                    productView: true, productAdd: false, productEdit: false, productDelete: false,
                    // Bank permissions
                    bankView: true, bankAdd: false, bankEdit: false, bankDelete: false,
                    // Branch permissions
                    branchView: true, branchAdd: false, branchEdit: false, branchDelete: false,
                    // Facility permissions
                    facilityView: true, facilityAdd: false, facilityEdit: false, facilityDelete: false,
                    // BuildingDetailRequest permissions
                    buildingDetailRequestView: true, buildingDetailRequestAdd: false, buildingDetailRequestEdit: false, buildingDetailRequestDelete: false,
                    // QuranicSchool permissions
                    quranicSchoolView: true, quranicSchoolAdd: false, quranicSchoolEdit: false, quranicSchoolDelete: false
                ),
                _ => Permission.Create(
                    // Default permissions for unknown roles - minimal read-only access
                    accountView: true, accountAdd: false, accountEdit: false, accountDelete: false,
                    accountDetailView: true, accountDetailAdd: false, accountDetailEdit: false, accountDetailDelete: false,
                    userView: false, userAdd: false, userEdit: false, userDelete: false,
                    roleView: false, roleAdd: false, roleEdit: false, roleDelete: false,
                    decisionView: true, decisionAdd: false, decisionEdit: false, decisionDelete: false,
                    requestView: true, requestAdd: false, requestEdit: false, requestDelete: false,
                    constructionRequestView: true, constructionRequestAdd: false, constructionRequestEdit: false, constructionRequestDelete: false,
                    maintenanceRequestView: true, maintenanceRequestAdd: false, maintenanceRequestEdit: false, maintenanceRequestDelete: false,
                    demolitionRequestView: true, demolitionRequestAdd: false, demolitionRequestEdit: false, demolitionRequestDelete: false,
                    nameChangeRequestView: true, nameChangeRequestAdd: false, nameChangeRequestEdit: false, nameChangeRequestDelete: false,
                    needsRequestView: true, needsRequestAdd: false, needsRequestEdit: false, needsRequestDelete: false,
                    expenditureChangeRequestView: true, expenditureChangeRequestAdd: false, expenditureChangeRequestEdit: false, expenditureChangeRequestDelete: false,
                    changeOfPathRequestView: true, changeOfPathRequestAdd: false, changeOfPathRequestEdit: false, changeOfPathRequestDelete: false,
                    officeView: true, officeAdd: false, officeEdit: false, officeDelete: false,
                    endowmentView: true, endowmentAdd: false, endowmentEdit: false, endowmentDelete: false,
                    cityView: true, cityAdd: false, cityEdit: false, cityDelete: false,
                    regionView: true, regionAdd: false, regionEdit: false, regionDelete: false,
                    buildingView: true, buildingAdd: false, buildingEdit: false, buildingDelete: false,
                    mosqueView: true, mosqueAdd: false, mosqueEdit: false, mosqueDelete: false,
                    productView: true, productAdd: false, productEdit: false, productDelete: false,
                    bankView: true, bankAdd: false, bankEdit: false, bankDelete: false,
                    branchView: true, branchAdd: false, branchEdit: false, branchDelete: false,
                    facilityView: true, facilityAdd: false, facilityEdit: false, facilityDelete: false,
                    buildingDetailRequestView: true, buildingDetailRequestAdd: false, buildingDetailRequestEdit: false, buildingDetailRequestDelete: false,
                    quranicSchoolView: true, quranicSchoolAdd: false, quranicSchoolEdit: false, quranicSchoolDelete: false
                )
            };
        }
    }
}
