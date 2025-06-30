using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Products.Commands.Delete
{
    public class DeleteProductCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}