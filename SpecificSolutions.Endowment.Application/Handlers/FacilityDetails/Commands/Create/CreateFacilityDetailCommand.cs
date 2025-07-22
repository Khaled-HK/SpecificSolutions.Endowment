using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Models.FacilityDetails;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Create
{
    public class CreateFacilityDetailCommand : ICommand, ICreateFacilityDetailCommand
    {
        public int Quantity { get; set; }

        public Guid ProductId { get; set; }

        public Guid BuildingDetailId { get; set; }
    }
}
