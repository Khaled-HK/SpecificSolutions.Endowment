using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.FacilityDetails;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Create
{
    [Authorize(Permissions = Permission.FacilityDetailAdd)]
    public class CreateFacilityDetailCommand : ICommand, ICreateFacilityDetailCommand
    {
        public int Quantity { get; set; }

        public Guid ProductId { get; set; }

        public Guid BuildingDetailId { get; set; }
    }
}
