using Microsoft.AspNetCore.Identity;
using SpecificSolutions.Endowment.Core.Entities.Accounts;
using SpecificSolutions.Endowment.Core.Entities.AuditLogs;
using SpecificSolutions.Endowment.Core.Entities.Buildings;
using SpecificSolutions.Endowment.Core.Entities.Decisions;
using SpecificSolutions.Endowment.Core.Entities.Offices;
using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Application.Models.Identity.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string? Name { get; private set; }
        public List<string> Permissions { get; private set; } = new List<string>();
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
        public virtual ICollection<Decision> Decisions { get; private set; } = new HashSet<Decision>();
        public virtual ICollection<Request> Requests { get; private set; } = new HashSet<Request>();
        public virtual ICollection<Account> Accounts { get; private set; } = new HashSet<Account>();
        public virtual ICollection<Building> Buildings { get; private set; } = new HashSet<Building>();
        public virtual ICollection<RefreshToken> IdentityUserTokens { get; private set; } = new HashSet<RefreshToken>();
        public virtual ICollection<AuditLog> AuditLogs { get; private set; } = new HashSet<AuditLog>();
        public Guid OfficeId { get; private set; }
        public virtual Office Office { get; private set; }

        // حالة الموافقة على المستخدم
        public bool IsApproved { get; private set; } = false;
        public DateTime? ApprovedAt { get; private set; }
        public string? ApprovedBy { get; private set; }

        // تاريخ إنشاء المستخدم - نمط خالد
        public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.Now;

        public ApplicationUser() { }

        private ApplicationUser(string email, string firstName, string lastName, Guid officeId, string userName, string passwordHash,
            bool emailConfirmed, bool isApproved, DateTime? approvedAt, string? approvedBy)
        {
            Id = Guid.NewGuid().ToString(); // تعيين Id تلقائياً
            Email = email;
            NormalizedEmail = email;
            UserName = userName;
            NormalizedUserName = userName;
            FirstName = firstName;
            LastName = lastName;
            OfficeId = officeId;
            Name = $"{firstName} {lastName}";
            PasswordHash = passwordHash;
            EmailConfirmed = emailConfirmed;
            SecurityStamp = Guid.NewGuid().ToString();
            IsApproved = isApproved;
            ApprovedAt = approvedAt;
            ApprovedBy = approvedBy;
        }

        // static factory method to create a new ApplicationUser
        public static ApplicationUser Create(string email, string firstName, string lastName, Guid officeId, string userName,
            string passwordHash, bool emailConfirmed, bool isApproved, DateTime? approvedAt, string? approvedBy)
        {
            return new ApplicationUser(email, firstName, lastName, officeId, userName, passwordHash, emailConfirmed,
                isApproved, approvedAt, approvedBy);
        }

        // seed method to create a new ApplicationUser
        public static ApplicationUser Seed(string id, string email, string firstName, string lastName, Guid officeId, string userName,
            string passwordHash, bool emailConfirmed, bool isApproved, DateTime? approvedAt, string? approvedBy)
        {
            var applicationUser = new ApplicationUser(email, firstName, lastName, officeId, userName, passwordHash,
               isApproved, emailConfirmed, approvedAt, approvedBy);

            applicationUser.Id = id; // تعيين Id محدد
            return applicationUser;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdateFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public void UpdateLastName(string lastName)
        {
            LastName = lastName;
        }

        public void AddPermission(string permission)
        {
            if (!Permissions.Contains(permission))
            {
                Permissions.Add(permission);
            }
        }

        public void RemovePermission(string permission)
        {
            if (Permissions.Contains(permission))
            {
                Permissions.Remove(permission);
            }
        }

        public void AddUserRole(ApplicationUserRole userRole)
        {
            UserRoles.Add(userRole);
        }

        public void RemoveUserRole(ApplicationUserRole userRole)
        {
            UserRoles.Remove(userRole);
        }

        public void AddDecision(Decision decision)
        {
            Decisions.Add(decision);
        }

        public void RemoveDecision(Decision decision)
        {
            Decisions.Remove(decision);
        }

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            IdentityUserTokens.Add(refreshToken);
        }

        public void RemoveRefreshToken(RefreshToken refreshToken)
        {
            IdentityUserTokens.Remove(refreshToken);
        }

        public void AddAuditLog(AuditLog auditLog)
        {
            AuditLogs.Add(auditLog);
        }

        public void RemoveAuditLog(AuditLog auditLog)
        {
            AuditLogs.Remove(auditLog);
        }

        public void AddPermissions(List<string> permissions)
        {
            // Clear existing permissions and add new ones to avoid duplicates
            Permissions.Clear();
            Permissions.AddRange(permissions.Distinct());
        }

        /// <summary>
        /// الموافقة على المستخدم ومنحه الصلاحيات
        /// </summary>
        public void ApproveUser(string approvedBy)
        {
            IsApproved = true;
            ApprovedAt = DateTime.UtcNow;
            ApprovedBy = approvedBy;
        }

        /// <summary>
        /// إلغاء الموافقة على المستخدم
        /// </summary>
        public void RejectUser()
        {
            IsApproved = false;
            ApprovedAt = null;
            ApprovedBy = null;
        }

        /// <summary>
        /// التحقق من حالة الموافقة
        /// </summary>
        public bool IsUserApproved()
        {
            return IsApproved;
        }
    }
}