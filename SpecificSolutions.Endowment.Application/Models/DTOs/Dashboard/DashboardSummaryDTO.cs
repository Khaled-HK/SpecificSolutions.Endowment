namespace SpecificSolutions.Endowment.Application.Models.DTOs.Dashboard
{
    public sealed class WeeklyPointDTO
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }

    public sealed class RegionCountDTO
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public sealed class DashboardSummaryDTO
    {
        // Core awqaf KPIs (non-financial)
        public int MosqueCount { get; set; }
        public int BuildingCount { get; set; }
        public int OfficesCount { get; set; }
        public int RegionsCount { get; set; }

        public int NewRequestsLast30Days { get; set; }
        public int UpcomingMaintenanceNext30Days { get; set; }

        public IReadOnlyList<RegionCountDTO> TopRegionsByMosques { get; set; } = new List<RegionCountDTO>();
        public IReadOnlyList<WeeklyPointDTO> WeeklyOverview { get; set; } = new List<WeeklyPointDTO>(); // requests per week
    }
}


