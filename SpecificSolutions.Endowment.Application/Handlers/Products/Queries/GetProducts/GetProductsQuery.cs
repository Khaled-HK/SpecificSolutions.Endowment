using MediatR;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Products.Queries.GetProducts
{
    public record GetProductsQuery : IRequest<EndowmentResponse<IEnumerable<KeyValuPair>>>;
} 