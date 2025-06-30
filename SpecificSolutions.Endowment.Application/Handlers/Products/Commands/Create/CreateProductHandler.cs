using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Products;

namespace SpecificSolutions.Endowment.Application.Handlers.Products.Commands.Create
{
    public class CreateProductHandler : ICommandHandler<CreateProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = Product.Create(request);

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}