using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Enums.BuildingDetails;
using SpecificSolutions.Endowment.Core.Models.BuildingDetails;
using System.Text.Json.Serialization;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Update
{
    public class UpdateBuildingDetailCommand : ICommand, IUpdateBuildingDetailCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Floors { get; set; }
        public bool WithinMosqueArea { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BuildingCategory BuildingCategory { get; set; } = BuildingCategory.Endowment;
    }
}