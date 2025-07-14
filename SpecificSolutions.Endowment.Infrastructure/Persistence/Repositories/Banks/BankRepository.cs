using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Banks.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Banks;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Banks;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Banks
{
    public class BankRepository : Repository<Bank>, IBankRepository
    {
        private readonly AppDbContext _context;

        public BankRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Bank> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Banks.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Bank>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Banks.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Bank bank, CancellationToken cancellationToken)
        {
            await _context.Banks.AddAsync(bank, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Bank bank)
        {
            _context.Banks.Update(bank);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var bank = await _context.Banks.FindAsync(id);
            if (bank != null)
            {
                _context.Banks.Remove(bank);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<BankDTO>> GetByFilterAsync(FilterBankQuery query, CancellationToken cancellationToken)
        {
            var banksQuery = _context.Banks.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                banksQuery = banksQuery.Where(b =>
                    b.Name.Contains(query.SearchTerm) ||
                    b.Address.Contains(query.SearchTerm));
            }

            var dtos = banksQuery.Select(b => new BankDTO
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                ContactNumber = b.ContactNumber
            });

            return await PagedList<BankDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}