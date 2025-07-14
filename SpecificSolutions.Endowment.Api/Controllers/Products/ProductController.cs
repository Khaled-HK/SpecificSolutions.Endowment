using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Products.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Products.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Products.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Products.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Products.Queries.GetProduct;
using SpecificSolutions.Endowment.Application.Handlers.Products.Queries.GetProducts;
using SpecificSolutions.Endowment.Application.Models.DTOs.Products;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Products
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProductController : ApiController
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateProductCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateProductCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteProductCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetProductById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetProductQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<ProductDTO>>> Filter([FromQuery] FilterProductQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetProducts")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetProducts([FromQuery] GetProductsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}