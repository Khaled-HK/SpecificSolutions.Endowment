using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.QuranicSchools;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.GetQuranicSchool
{
    public record GetQuranicSchoolQuery(Guid Id) : IRequest<EndowmentResponse<QuranicSchoolDTO>>;
} 