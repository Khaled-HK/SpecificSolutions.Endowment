namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IRepository<TDomain> where TDomain : class
    {
        Task AddAsync(TDomain entity);
        Task AddRangeAsync(IList<TDomain> entities);
        Task RemoveAsync(TDomain entity);
        Task RemoveRangeAsync(IList<TDomain> entities);
        Task<TDomain> FindAsync(Guid id, bool includeRelated = false);
        Task<IEnumerable<TDomain>> GetAllAsync();
        Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<TDomain, TResult>> target);
    }
}