using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Branches.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Branchs;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Branchs;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Branchs
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        private readonly AppDbContext _context;

        public BranchRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Branch> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Branches.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Branch>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Branches.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Branch branch, CancellationToken cancellationToken)
        {
            await _context.Branches.AddAsync(branch, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Branch branch)
        {
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                _context.Branches.Remove(branch);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<BranchDTO>> GetByFilterAsync(FilterBranchQuery query, CancellationToken cancellationToken)
        {
            var branchesQuery = _context.Branches.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                branchesQuery = branchesQuery.Where(b =>
                    b.Name.Contains(query.SearchTerm) ||
                    b.Address.Contains(query.SearchTerm));
            }

            var dtos = branchesQuery.Select(b => new BranchDTO
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                ContactNumber = b.ContactNumber,
                BankId = b.BankId
            });

            return await PagedList<BranchDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}