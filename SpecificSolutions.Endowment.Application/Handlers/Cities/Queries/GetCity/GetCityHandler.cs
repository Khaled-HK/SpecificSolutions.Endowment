using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Cities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.GetCity
{
    public class GetCityHandler : IRequestHandler<GetCityQuery, EndowmentResponse<CityDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<CityDTO>> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Cities.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return Response.FailureResponse<CityDTO>("The specified city could not be located. Please verify the ID and try again.");
            }

            var dto = new CityDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Country = entity.Country
            };

            return new(data: dto);
        }
    }
} 