using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.FacilityDetails;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Create
{
    public class CreateFacilityDetailHandler : ICommandHandler<CreateFacilityDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFacilityDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateFacilityDetailCommand request, CancellationToken cancellationToken)
        {
            // تحقق من أن BuildingDetailId ليس فارغًا
            if (request.BuildingDetailId == Guid.Empty)
            {
                return Response.FailureResponse("BuildingDetailId", "معرف تفاصيل المبنى غير صالح.");
            }

            // تحقق من وجود المنتج
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
            {
                return Response.FailureResponse("ProductId", "المادة غير موجودة في قاعدة البيانات.");
            }

            // تحقق من وجود تفاصيل المبنى
            var buildingDetail = await _unitOfWork.BuildingDetails.GetByIdAsync(request.BuildingDetailId, cancellationToken);
            if (buildingDetail == null)
            {
                return Response.FailureResponse("BuildingDetailId", "تفاصيل المبنى غير موجودة في قاعدة البيانات.");
            }

            // تحقق من عدم تكرار نفس المادة لنفس تفاصيل المبنى
            var exists = _unitOfWork.FacilityDetails
                .GetAllAsync(cancellationToken)
                .Result
                .Any(fd => fd.BuildingDetailId == request.BuildingDetailId && fd.ProductId == request.ProductId);
            if (exists)
            {
                return Response.FailureResponse("ProductId", "هذه المادة مضافة مسبقًا لتفاصيل المبنى.");
            }

            var facilityDetail = FacilityDetail.Create(request);

            await _unitOfWork.FacilityDetails.AddAsync(facilityDetail, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}