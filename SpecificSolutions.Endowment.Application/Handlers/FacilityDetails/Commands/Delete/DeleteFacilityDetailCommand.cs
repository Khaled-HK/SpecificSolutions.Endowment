using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Delete
{
    public class DeleteFacilityDetailCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}