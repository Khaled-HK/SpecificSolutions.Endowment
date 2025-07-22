using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Models.FacilityDetails;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Update
{
    public class UpdateFacilityDetailCommand : ICommand, IUpdateFacilityDetailCommand
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}