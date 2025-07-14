using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Queries.Filter;

public class FilterBuildingDetailHandler : IQueryHandler<FilterBuildingDetailQuery, PagedList<BuildingDetailDTO>>
{
    private readonly IUnitOfWork _unitOfWork;

    public FilterBuildingDetailHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EndowmentResponse<PagedList<BuildingDetailDTO>>> Handle(FilterBuildingDetailQuery query, CancellationToken cancellationToken)
    {
        var requests = await _unitOfWork.BuildingDetails.GetByFilterAsync(query, cancellationToken);

        if (!requests.Items.Any())
            return Response.FilterResponse<PagedList<BuildingDetailDTO>>(PagedList<BuildingDetailDTO>.Empty());

        return Response.FilterResponse<PagedList<BuildingDetailDTO>>(requests);
    }
}