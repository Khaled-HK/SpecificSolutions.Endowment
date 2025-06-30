using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Delete
{
    public class DeleteMosqueCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}