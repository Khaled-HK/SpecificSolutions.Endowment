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

        public virtual async Task AddAsync(TDomain entity, CancellationToken cancellationToken)
        {
            await _context.Set<TDomain>().AddAsync(entity, cancellationToken);
        }

        public virtual async Task AddRangeAsync(IList<TDomain> entities, CancellationToken cancellationToken)
        {
            await _context.Set<TDomain>().AddRangeAsync(entities, cancellationToken);
        }

        public virtual async Task<TDomain?> FindAsync(Guid id, CancellationToken cancellationToken, bool includeRelated = false)
        {
            return await _context.Set<TDomain>().FindAsync(id, cancellationToken);
        }

        public virtual async Task<IEnumerable<TDomain>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<TDomain>().ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<TDomain, TResult>> target, CancellationToken cancellationToken)
        {
            return await _context.Set<TDomain>().Select(target).ToListAsync(cancellationToken);
        }

        public virtual async Task RemoveAsync(TDomain entity)
        {
            _context.Set<TDomain>().Remove(entity);
        }

        public virtual async Task RemoveRangeAsync(IList<TDomain> entities)
        {
            _context.Set<TDomain>().RemoveRange(entities);
        }
    }
}