using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.Products;

namespace SpecificSolutions.Endowment.Application.Handlers.Products.Commands.Create
{
    [Authorize(Permissions = Permission.ProductAdd)]
    public class CreateProductCommand : ICommand, ICreateProductCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}