using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Products.Commands.Delete
{
    [Authorize(Permissions = Permission.ProductDelete)]
    public class DeleteProductCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}