using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Products;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Products.Queries.GetProduct
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, EndowmentResponse<ProductDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetProductHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<ProductDTO>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                return Response.FailureResponse<ProductDTO>("The specified product could not be located. Please verify the ID and try again.");
            var dto = new ProductDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
            return new(data: dto);
        }
    }
} 