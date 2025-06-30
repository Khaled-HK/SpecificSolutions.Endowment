using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.AccountDetails;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.AccountDetails;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.AccountDetails
{
    public class AccountDetailRepository : Repository<AccountDetail>, IAccountDetailRepository
    {
        private readonly AppDbContext _context;

        public AccountDetailRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AccountDetail> GetByIdAsync(Guid id)
        {
            return await _context.AccountDetails.FindAsync(id);
        }

        public async Task AddAsync(AccountDetail accountDetail)
        {
            await _context.AccountDetails.AddAsync(accountDetail);
        }

        public async Task UpdateAsync(AccountDetail accountDetail)
        {
            _context.AccountDetails.Update(accountDetail);
        }

        public async Task RemoveAsync(AccountDetail accountDetail)
        {
            _context.AccountDetails.Remove(accountDetail);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.AccountDetails.AnyAsync(ad => ad.Id == id);
        }

        // Implement the new method
        public async Task<AccountDetail?> GetByAccountDetailIdAsync(Guid accountDetailId)
        {
            return await _context.AccountDetails
                .FirstOrDefaultAsync(ad => ad.Id == accountDetailId);
        }

        public async Task<PagedList<FilterAccountDetailDTO>> GetByFilterAsync(FilterAccountDetailQuery query, CancellationToken cancellationToken)
        {
            var accountsQuery = _context.AccountDetails.AsQueryable();

            // Apply filtering based on the query parameters

            // Select the relevant fields to return as DTOs
            var accountDTOs = accountsQuery.Select(a => new FilterAccountDetailDTO
            {
                Id = a.Id,
                Debtor = a.Debtor,
                Creditor = a.Creditor,
                Date = a.CreatedDate,
                Note = a.Note,
                OperationType = a.OperationType,
                OperationNumber = a.OperationNumber,
                Balance = a.Balance,
                AccountId = a.AccountId
            });

            // Return paged results
            return await PagedList<FilterAccountDetailDTO>.CreateAsync(accountDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}