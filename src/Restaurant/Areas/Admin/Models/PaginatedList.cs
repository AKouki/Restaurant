using Microsoft.EntityFrameworkCore;

namespace Restaurant.Areas.Admin.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public PaginatedList()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            // Correction (in case using the constructor directly)
            if (pageNumber < 1 || pageNumber > Math.Ceiling((decimal)count / pageSize))
                pageNumber = 1;

            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = count;

            AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();

            // Correction
            if (pageNumber < 1 || pageNumber > Math.Ceiling((decimal)count / pageSize))
                pageNumber = 1;

            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
