using SpecificSolutions.Endowment.Application.Handlers.Products.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Products.Queries.GetProducts;
using SpecificSolutions.Endowment.Application.Models.DTOs.Products;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Products;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(Product product, CancellationToken cancellationToken);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
        Task<PagedList<ProductDTO>> GetByFilterAsync(FilterProductQuery query, CancellationToken cancellationToken);
        Task<IEnumerable<KeyValuPair>> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken);
    }
}