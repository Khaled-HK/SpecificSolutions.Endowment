using Microsoft.EntityFrameworkCore;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Dashboard.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Dashboard;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Reports
{
    public sealed class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDTO> GetByFilterAsync(FilterDashboardQuery query, CancellationToken cancellationToken)
        {
            var to = query.To ?? DateTime.UtcNow;
            var from = query.From ?? to.AddDays(-30);

            // KPIs غير مالية
            var mosqueCount = await _context.Mosques
                .AsNoTracking()
                .CountAsync(cancellationToken);

            var buildingCount = await _context.Buildings
                .AsNoTracking()
                .CountAsync(cancellationToken);

            var officesCount = await _context.Offices
                .AsNoTracking()
                .CountAsync(cancellationToken);

            var regionsCount = await _context.Regions
                .AsNoTracking()
                .CountAsync(cancellationToken);

            var newRequestsLast30Days = await _context.Requests
                .AsNoTracking()
                .Where(r => r.CreatedDate >= from && r.CreatedDate <= to)
                .CountAsync(cancellationToken);

            var upcomingMaintenanceNext30Days = await _context.MaintenanceRequests
                .AsNoTracking()
                .Where(m => m.ExpectedStartDate <= to.AddDays(30) && m.ExpectedEndDate >= to)
                .CountAsync(cancellationToken);

            // WeeklyOverview: طلبات جديدة لكل أسبوع من آخر 30 يوم
            var weeklyRaw = await _context.Requests
                .AsNoTracking()
                .Where(x => x.CreatedDate >= from && x.CreatedDate <= to)
                .GroupBy(x => EF.Functions.DateDiffWeek(from, x.CreatedDate))
                .Select(g => new
                {
                    WeekIndex = g.Key,
                    Value = (decimal)g.Count()
                })
                .OrderBy(x => x.WeekIndex)
                .ToListAsync(cancellationToken);

            var weekly = weeklyRaw
                .Select(w => new WeeklyPointDTO
                {
                    Label = $"W{w.WeekIndex}",
                    Value = w.Value
                })
                .ToList();

            // أفضل المناطق بعدد المساجد
            var topRegions = await _context.Mosques
                .AsNoTracking()
                .Include(m => m.Building)
                .ThenInclude(b => b.Region)
                .GroupBy(m => m.Building.Region.Name)
                .Select(g => new RegionCountDTO { Name = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync(cancellationToken);

            return new DashboardSummaryDTO
            {
                MosqueCount = mosqueCount,
                BuildingCount = buildingCount,
                OfficesCount = officesCount,
                RegionsCount = regionsCount,
                NewRequestsLast30Days = newRequestsLast30Days,
                UpcomingMaintenanceNext30Days = upcomingMaintenanceNext30Days,
                WeeklyOverview = weekly,
                TopRegionsByMosques = topRegions,
            };
        }
    }
}


