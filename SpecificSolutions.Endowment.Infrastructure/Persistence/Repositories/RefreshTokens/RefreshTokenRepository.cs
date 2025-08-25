using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.RefreshTokens;
public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    private readonly AppDbContext _context;
    public RefreshTokenRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.IdentityUserToken.FirstOrDefaultAsync(rt => rt.Token == token);
    }


    public async Task<bool> ExistsAsync(string token)
    {
        return await _context.IdentityUserToken.AnyAsync(rt => rt.Token == token);
    }

    // Delete by user id
    public async Task RemoveByUserIdAsync(string userId)
    {
        await _context.IdentityUserToken.Where(rt => rt.UserId == userId).ExecuteDeleteAsync();
    }
}
