using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.UserApprovals;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ApplicationUsers
{
    /// <summary>
    /// Repository للمستخدمين - نمط خالد
    /// مسؤول فقط عن العمليات الأساسية (CRUD) على ApplicationUser
    /// </summary>
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly AppDbContext _context;

        public ApplicationUserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// الحصول على مستخدم بواسطة ID (string)
        /// </summary>
        public async Task<ApplicationUser?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FindAsync(userId);
        }

        /// <summary>
        /// الحصول على مستخدم بواسطة Email
        /// </summary>
        public async Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        /// <summary>
        /// الحصول على مستخدم بواسطة UserName
        /// </summary>
        public async Task<ApplicationUser?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        }

        /// <summary>
        /// التحقق من وجود مستخدم بواسطة Email
        /// </summary>
        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email, cancellationToken);
        }

        /// <summary>
        /// التحقق من وجود مستخدم بواسطة UserName
        /// </summary>
        public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AnyAsync(u => u.UserName == userName, cancellationToken);
        }

        /// <summary>
        /// الحصول على جميع المستخدمين مع Pagination
        /// </summary>
        public async Task<List<ApplicationUser>> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// الحصول على عدد جميع المستخدمين
        /// </summary>
        public async Task<int> GetAllUsersCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users.CountAsync(cancellationToken);
        }

        /// <summary>
        /// الحصول على المستخدمين المعلقين مع Pagination - نمط خالد
        /// تنفيذ الفلترة والتحويل إلى DTO على مستوى قاعدة البيانات
        /// </summary>
        public async Task<List<PendingUserDto>> GetPendingUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var users = await _context.Users
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
        /// الحصول على عدد المستخدمين المعلقين - نمط خالد
        /// تنفيذ الفلترة على مستوى قاعدة البيانات
        /// </summary>
        public async Task<int> GetPendingUsersCountAsync(
            string? searchTerm = null,
            string? officeId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Users.Where(u => !u.IsApproved);

            // فلترة حسب مصطلح البحث
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u => 
                    u.FirstName.Contains(searchTerm) ||
                    u.LastName.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm) ||
                    u.UserName.Contains(searchTerm));
            }

            // فلترة حسب المكتب
            if (!string.IsNullOrWhiteSpace(officeId) && Guid.TryParse(officeId, out var officeGuid))
            {
                query = query.Where(u => u.OfficeId == officeGuid);
            }

            // فلترة حسب التاريخ
            if (fromDate.HasValue)
            {
                query = query.Where(u => u.CreatedDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(u => u.CreatedDate <= toDate.Value);
            }

            return await query.CountAsync(cancellationToken);
        }
    }
}
