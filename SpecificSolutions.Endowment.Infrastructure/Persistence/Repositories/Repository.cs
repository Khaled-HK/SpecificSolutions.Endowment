using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using System.Linq.Expressions;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories
{
    public class Repository<TDomain> : IRepository<TDomain> where TDomain : class
    {
        private readonly AppDbContext _context; // Use AppDBContext instead of ERPDbContext

        protected Repository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(TDomain entity)
        {
            await _context.Set<TDomain>().AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IList<TDomain> entities)
        {
            await _context.Set<TDomain>().AddRangeAsync(entities);
        }

        public virtual async Task<TDomain?> FindAsync(Guid id, bool includeRelated = false)
        {
            return await _context.Set<TDomain>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<TDomain>> GetAllAsync()
        {
            return await _context.Set<TDomain>().ToListAsync();
        }

        public virtual async Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<TDomain, TResult>> target)
        {
            return await _context.Set<TDomain>().Select(target).ToListAsync();
        }

        public virtual async Task RemoveAsync(TDomain entity)
        {
            _context.Set<TDomain>().Remove(entity);
            await _context.SaveChangesAsync(); // Ensure changes are saved
        }

        public virtual async Task RemoveRangeAsync(IList<TDomain> entities)
        {
            _context.Set<TDomain>().RemoveRange(entities);
            await _context.SaveChangesAsync(); // Ensure changes are saved
        }
    }
}