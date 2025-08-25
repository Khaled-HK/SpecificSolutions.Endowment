using Microsoft.EntityFrameworkCore;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Core.Entities;
using SpecificSolutions.Endowment.Infrastructure.Persistence;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories
{
    public class VerificationCodeRepository : IVerificationCodeRepository
    {
        private readonly ApplicationDbContext _context;

        public VerificationCodeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VerificationCode?> GetValidCodeAsync(string email, string code, string purpose)
        {
            return await _context.VerificationCodes
                .Where(vc => vc.Email == email && 
                            vc.Code == code && 
                            vc.Purpose == purpose &&
                            vc.ExpiresAt > DateTime.UtcNow &&
                            !vc.IsUsed &&
                            !vc.IsExpired)
                .FirstOrDefaultAsync();
        }

        public async Task<VerificationCode?> GetLatestValidCodeAsync(string email, string purpose)
        {
            return await _context.VerificationCodes
                .Where(vc => vc.Email == email && 
                            vc.Purpose == purpose &&
                            vc.ExpiresAt > DateTime.UtcNow &&
                            !vc.IsUsed &&
                            !vc.IsExpired)
                .OrderByDescending(vc => vc.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<VerificationCode> CreateAsync(VerificationCode verificationCode)
        {
            _context.VerificationCodes.Add(verificationCode);
            await _context.SaveChangesAsync();
            return verificationCode;
        }

        public async Task<bool> UpdateAsync(VerificationCode verificationCode)
        {
            _context.VerificationCodes.Update(verificationCode);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var verificationCode = await _context.VerificationCodes.FindAsync(id);
            if (verificationCode == null)
                return false;

            _context.VerificationCodes.Remove(verificationCode);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteExpiredCodesAsync()
        {
            var expiredCodes = await _context.VerificationCodes
                .Where(vc => vc.ExpiresAt <= DateTime.UtcNow || vc.IsUsed || vc.IsExpired)
                .ToListAsync();

            _context.VerificationCodes.RemoveRange(expiredCodes);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetActiveCodesCountAsync(string email, string purpose)
        {
            return await _context.VerificationCodes
                .CountAsync(vc => vc.Email == email && 
                                 vc.Purpose == purpose &&
                                 vc.ExpiresAt > DateTime.UtcNow &&
                                 !vc.IsUsed &&
                                 !vc.IsExpired);
        }

        public async Task<bool> InvalidatePreviousCodesAsync(string email, string purpose)
        {
            var previousCodes = await _context.VerificationCodes
                .Where(vc => vc.Email == email && 
                            vc.Purpose == purpose &&
                            !vc.IsUsed &&
                            !vc.IsExpired)
                .ToListAsync();

            foreach (var code in previousCodes)
            {
                code.MarkAsExpired();
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<VerificationCode?> GetByIdAsync(int id)
        {
            return await _context.VerificationCodes.FindAsync(id);
        }

        public async Task<IEnumerable<VerificationCode>> GetByEmailAsync(string email, string purpose)
        {
            return await _context.VerificationCodes
                .Where(vc => vc.Email == email && vc.Purpose == purpose)
                .OrderByDescending(vc => vc.CreatedAt)
                .ToListAsync();
        }
    }
}
