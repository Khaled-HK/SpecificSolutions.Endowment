using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Accounts;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Accounts;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Accounts
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<PagedList<FilterAccountDTO>> GetByFilterAsync(FilterAccountQuery query, CancellationToken cancellationToken)
        {
            var accountsQuery = _context.Accounts.AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
            {
                accountsQuery = accountsQuery.Where(a => a.Name.Contains(query.Name));
            }

            if (!string.IsNullOrEmpty(query.MotherName))
            {
                accountsQuery = accountsQuery.Where(a => a.MotherName.Contains(query.MotherName));
            }

            if (query.BirthDate.HasValue)
            {
                accountsQuery = accountsQuery.Where(a => a.BirthDate == query.BirthDate.Value);
            }

            if (query.Gender.HasValue)
            {
                accountsQuery = accountsQuery.Where(a => a.Gender == query.Gender.Value);
            }

            var accountDTOs = accountsQuery.Select(a => new FilterAccountDTO
            {
                Id = a.Id,
                Name = a.Name,
                MotherName = a.MotherName,
                BirthDate = a.BirthDate,
                Gender = a.Gender,
                Barcode = a.Barcode,
                Status = a.Status,
                LockerFileNumber = a.LockerFileNumber,
                SocialStatus = a.SocialStatus,
                BookNumber = a.BookNumber,
                PaperNumber = a.PaperNumber,
                RegistrationNumber = a.RegistrationNumber,
                AccountNumber = a.AccountNumber,
                Type = a.Type,
                LookOver = a.LookOver,
                Note = a.Note,
                NID = a.NID,
                IsActive = a.IsActive,
                Balance = a.Balance
            }).AsQueryable();

            return await PagedList<FilterAccountDTO>.CreateAsync(accountDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}