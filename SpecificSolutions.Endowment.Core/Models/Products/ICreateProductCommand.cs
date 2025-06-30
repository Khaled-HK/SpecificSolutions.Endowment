
namespace SpecificSolutions.Endowment.Core.Models.Products
{
    public interface ICreateProductCommand
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
