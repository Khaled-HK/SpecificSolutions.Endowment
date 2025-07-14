using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Queries.GetAccountDetails
{
    public class GetAccountDetailsHandler : IRequestHandler<GetAccountDetailsQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAccountDetailsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetAccountDetailsQuery request, CancellationToken cancellationToken)
        {
            var accountDetails = await _unitOfWork.AccountDetails.GetAllAsync(cancellationToken);

            var result = accountDetails.Select(ad => new KeyValuPair
            {
                Key = ad.Id,
                Value = ad.Note
            });

            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
}