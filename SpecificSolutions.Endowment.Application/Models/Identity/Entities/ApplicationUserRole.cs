using Microsoft.AspNetCore.Identity;

namespace SpecificSolutions.Endowment.Application.Models.Identity.Entities
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationRole Role { get; set; }

        public Permission Permissions { get; private set; }

        public ApplicationUserRole() { }

        private ApplicationUserRole(string userId, string roleId, Permission permissions)
        {
            UserId = userId;
            RoleId = roleId;
            Permissions = permissions;
        }

        public static ApplicationUserRole Create(string userId, string roleId, Permission permissions)
        {
            return new ApplicationUserRole(userId, roleId, permissions);
        }

        //public void AddPermission(string permission)
        //{
        //    if (string.IsNullOrWhiteSpace(permission))
        //        throw new ArgumentException("Permission cannot be null or empty", nameof(permission));

        //    if (!Permissions.Contains(permission))
        //        Permissions.Add(permission);
        //}

        //public void RemovePermission(string permission)
        //{
        //    if (string.IsNullOrWhiteSpace(permission))
        //        throw new ArgumentException("Permission cannot be null or empty", nameof(permission));

        //    if (Permissions.Contains(permission))
        //        Permissions.Remove(permission);
        //}
    }
}
