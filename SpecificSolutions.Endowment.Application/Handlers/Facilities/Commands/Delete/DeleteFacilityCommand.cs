using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Delete
{
    public class DeleteFacilityCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}