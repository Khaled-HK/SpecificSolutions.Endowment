using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Update
{
    public class UpdateFacilityDetailHandler : ICommandHandler<UpdateFacilityDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFacilityDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateFacilityDetailCommand request, CancellationToken cancellationToken)
        {
            var facilityDetail = await _unitOfWork.FacilityDetails.GetByIdAsync(request.Id, cancellationToken);
            if (facilityDetail == null)
                return Response.FailureResponse("FacilityDetail not found.");

            // تحقق من وجود المنتج
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
            {
                return Response.FailureResponse("ProductId", "المادة غير موجودة في قاعدة البيانات.");
            }

            // تحقق من صحة الكمية
            if (request.Quantity <= 0)
            {
                return Response.FailureResponse("Quantity", "الكمية يجب أن تكون أكبر من صفر.");
            }

            // Update the facility detail with new values
            facilityDetail.Update(request);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}