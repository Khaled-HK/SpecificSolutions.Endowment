using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Mosques;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.Filter;

public class FilterMosqueHandler : IQueryHandler<FilterMosqueQuery, PagedList<MosqueDTO>>
{
    private readonly IUnitOfWork _unitOfWork;

    public FilterMosqueHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EndowmentResponse<PagedList<MosqueDTO>>> Handle(FilterMosqueQuery query, CancellationToken cancellationToken)
    {
        var accountDetails = await _unitOfWork.Mosques.GetByFilterAsync(query, cancellationToken);

        if (!accountDetails.Items.Any())
            return Response.FilterResponse<PagedList<MosqueDTO>>(PagedList<MosqueDTO>.Empty());

        return Response.FilterResponse<PagedList<MosqueDTO>>(accountDetails);
    }
}