using SpecificSolutions.Endowment.Core.Entities;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IVerificationCodeRepository
    {
        Task<VerificationCode?> GetValidCodeAsync(string email, string code, string purpose);
        Task<VerificationCode?> GetLatestValidCodeAsync(string email, string purpose);
        Task<VerificationCode> CreateAsync(VerificationCode verificationCode);
        Task<bool> UpdateAsync(VerificationCode verificationCode);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteExpiredCodesAsync();
        Task<int> GetActiveCodesCountAsync(string email, string purpose);
        Task<bool> InvalidatePreviousCodesAsync(string email, string purpose);
        Task<VerificationCode?> GetByIdAsync(int id);
        Task<IEnumerable<VerificationCode>> GetByEmailAsync(string email, string purpose);
    }
}
