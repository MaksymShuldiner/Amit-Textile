using System;

namespace AmitTextile.Models
{
    public class PageViewModel
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; private set; }

        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
        public bool HasPreviousPage => (PageNumber > 1);

        public bool HasNextPage => (PageNumber < TotalPages);

        public bool UniversalHasNextPage(int pagenumber)
        {
            return pagenumber < TotalPages;
        }

        public bool UniversalHasPreviousPage(int pagenumber)
        {
            return pagenumber > 1;
        }

    }
}