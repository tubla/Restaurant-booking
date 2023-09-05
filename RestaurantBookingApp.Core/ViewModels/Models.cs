namespace RestaurantBookingApp.Core.ViewModels
{
    public class RestaurantModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class RestaurantBranchModel
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class DiningTableWithTimeSlotstModel
    {
        public int BranchId { get; set; }
        public int TimeSlotId { get; set; }
        public DateTime ResevationDay { get; set; }
        public string? TableName { get; set; }
        public int Capacity { get; set; }
        public string MealType { get; set; } = null!;
        public string TableStatus { get; set; } = null!;
    }

    public class BlobDownloadModel
    {
        public string? Name { get; set; }
        public string DownloadLink { get; set; } = null!;
    }

    public class PagingParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }

    public class PagedResponse<T>
    {
        /* Formulae : Skip((PageNumber - 1) * PageSize).Take(pageSize) */
        /* PageNumber - Which page number to be displayed - for example, 3 means the 3rd page  */
        /* PageSize - Number of records to be displayed in the 3rd page - for example, 10 means the 10 records to be shown in 3rd page  */
        /* So if user wants 10 records to be displayed in 3rd page, then we need to skip first 20 records of first and second page respectively, and show the record from 21 onwards */
        /* Therefore Skip -> (3 - 1) * 10 = 20 and then take 10 means next 10 records */

        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public List<T> Data { get; private set; }

        public PagedResponse(List<T> items, int totalRecordsCount, int currentPageNumber, int pageSize)
        {
            Data = items;

            TotalCount = totalRecordsCount;
            CurrentPage = currentPageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalRecordsCount / (double)pageSize);
        }
    }

    public class PagingFilter<T>
    {
        public static PagedResponse<T> ToPagedResponse(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var totalRecords = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResponse<T>(items, totalRecords, pageNumber, pageSize);
        }
    }
}
