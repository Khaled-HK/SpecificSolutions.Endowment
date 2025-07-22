using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Products.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Products.Queries.GetProducts;
using SpecificSolutions.Endowment.Application.Models.DTOs.Products;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Products;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Products.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Products.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<ProductDTO>> GetByFilterAsync(FilterProductQuery query, CancellationToken cancellationToken)
        {
            var productsQuery = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                productsQuery = productsQuery.Where(p =>
                    p.Name.Contains(query.SearchTerm) ||
                    p.Description.Contains(query.SearchTerm));
            }

            var dtos = productsQuery.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            });

            return await PagedList<ProductDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
        }

        public async Task<IEnumerable<KeyValuPair>> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var productsQuery = _context.Products.AsQueryable();
            // Apply filtering based on the query parameters
            if (!string.IsNullOrEmpty(query.Name))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(query.Name));
            }
            // Select the relevant fields to return as DTOs
            var productDTOs = productsQuery.Select(p => new KeyValuPair
            {
                Key = p.Id,
                Value = p.Name,
            });
            // Return paged results
            return await productDTOs.ToListAsync();
        }
    }
}