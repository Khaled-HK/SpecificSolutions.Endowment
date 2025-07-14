using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.QuranicSchools;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.GetQuranicSchool
{
    public class GetQuranicSchoolHandler : IRequestHandler<GetQuranicSchoolQuery, EndowmentResponse<QuranicSchoolDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetQuranicSchoolHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<QuranicSchoolDTO>> Handle(GetQuranicSchoolQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.QuranicSchools.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                return Response.FailureResponse<QuranicSchoolDTO>("The specified Quranic school could not be located. Please verify the ID and try again.");
            var dto = new QuranicSchoolDTO
            {
                Id = entity.Id,
                Name = entity.Building.Name,
                Location = entity.Building.NearestLandmark,
                ContactInfo = entity.Building.BriefDescription,
                NumberOfStudents = int.TryParse(entity.Building.PrayerCapacity, out var n) ? n : 0,
                Status = "Active" // Default value since it's not in entity
            };
            return new(data: dto);
        }
    }
} 