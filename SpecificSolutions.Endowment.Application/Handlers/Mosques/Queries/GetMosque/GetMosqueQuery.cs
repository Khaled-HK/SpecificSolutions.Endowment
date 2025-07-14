using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Mosques;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.GetMosque
{
    public record GetMosqueQuery(Guid Id) : IRequest<EndowmentResponse<MosqueDTO>>;
} 