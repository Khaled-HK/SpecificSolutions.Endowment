using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.GetCities
{
    public class GetCitiesHandler : IRequestHandler<GetCitiesQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCitiesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Cities.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair
            {
                Key = e.Id,
                Value = e.Name
            });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 