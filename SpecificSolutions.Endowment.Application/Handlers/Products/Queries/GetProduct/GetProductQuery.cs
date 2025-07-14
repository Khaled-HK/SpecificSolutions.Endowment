using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Products;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Products.Queries.GetProduct
{
    public record GetProductQuery(Guid Id) : IRequest<EndowmentResponse<ProductDTO>>;
} 