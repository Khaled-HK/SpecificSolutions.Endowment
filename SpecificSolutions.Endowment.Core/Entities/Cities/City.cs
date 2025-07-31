using SpecificSolutions.Endowment.Core.Models;

namespace SpecificSolutions.Endowment.Core.Entities.Cities;

public class City : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string RegionId { get; set; } = string.Empty;
    public int Population { get; set; }
    public decimal Area { get; set; }
    public string Mayor { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime EstablishedDate { get; set; }
    public string Coordinates { get; set; } = string.Empty;
}