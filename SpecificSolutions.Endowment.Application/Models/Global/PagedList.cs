using Microsoft.EntityFrameworkCore;

namespace SpecificSolutions.Endowment.Application.Models.Global
{
    public class PagedList<T>
    {
        public IEnumerable<T> Items { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }
        public int TotalCount { get; } // Total number of records
        public int TotalPages { get; } // Total number of pages

        private PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            HasPreviousPage = pageNumber > 1;
            HasNextPage = pageNumber < TotalPages;
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            // Get total count of records before pagination
            var count = await query.CountAsync(cancellationToken);
            // Get the items for the current page
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static PagedList<T> Empty()
        {
            return new([], 0, 1, 0);
        }
    }
}