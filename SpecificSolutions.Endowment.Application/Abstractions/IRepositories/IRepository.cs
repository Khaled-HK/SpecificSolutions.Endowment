namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IRepository<TDomain> where TDomain : class
    {
        Task AddAsync(TDomain entity, CancellationToken cancellationToken);
        Task AddRangeAsync(IList<TDomain> entities, CancellationToken cancellationToken);
        Task RemoveAsync(TDomain entity);
        Task RemoveRangeAsync(IList<TDomain> entities);
        Task<TDomain> FindAsync(Guid id, CancellationToken cancellationToken, bool includeRelated = false);
        Task<IEnumerable<TDomain>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<TDomain, TResult>> target, CancellationToken cancellationToken);
    }
}