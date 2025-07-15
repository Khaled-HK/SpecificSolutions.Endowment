using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Products;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Products.Queries.Filter
{
    public class FilterProductHandler : IQueryHandler<FilterProductQuery, PagedList<ProductDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterProductHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<ProductDTO>>> Handle(FilterProductQuery query, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.Products.GetByFilterAsync(query, cancellationToken);

            if (!requests.Items.Any())
            {
                return Response.FilterResponse(PagedList<ProductDTO>.Empty());
            }

            return Response.FilterResponse(requests);
        }
    }
}