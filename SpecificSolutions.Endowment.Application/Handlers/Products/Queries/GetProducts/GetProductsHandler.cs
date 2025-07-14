using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Products.Queries.GetProducts
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetProductsHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Products.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair { Key = e.Id, Value = e.Name });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 