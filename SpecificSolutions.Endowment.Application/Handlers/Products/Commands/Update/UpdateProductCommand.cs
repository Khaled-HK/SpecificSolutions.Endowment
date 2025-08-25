using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.Products;

namespace SpecificSolutions.Endowment.Application.Handlers.Products.Commands.Update
{
    [Authorize(Permissions = Permission.ProductUpdate)]
    public class UpdateProductCommand : ICommand, IUpdateProductCommand
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}