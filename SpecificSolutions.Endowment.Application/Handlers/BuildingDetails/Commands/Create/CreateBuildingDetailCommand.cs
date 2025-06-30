using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Enums.BuildingDetails;
using SpecificSolutions.Endowment.Core.Models.BuildingDetails;
using System.Text.Json.Serialization;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Create
{
    public class CreateBuildingDetailCommand : ICommand, ICreateBuildingDetailCommand
    {
        public string Name { get; set; }
        public Guid MosqueID { get; set; }
        public int Floors { get; set; }
        public bool WithinMosqueArea { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))] // Allows both names and numbers
        public BuildingCategory BuildingCategory { get; set; } = BuildingCategory.Endowment;
        //public HashSet<ICreateFacilityDetailCommand> CreateFacilityDetailCommands { get; set; }
    }
}