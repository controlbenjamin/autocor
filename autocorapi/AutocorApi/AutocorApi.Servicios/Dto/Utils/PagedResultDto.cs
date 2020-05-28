using System.Collections.Generic;

namespace AutocorApi.Servicios.Dto.Utils
{
    public class PagedResultDto<T> where T : class
    {
        public PagedResultDto(IEnumerable<T> items, int pageNumber, int pageSize, int totalItemsCount)
        {
            this.Items = items;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalItemsCount = totalItemsCount;
        }

        public IEnumerable<T> Items { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }

        public int TotalItemsCount { get; private set; }

        public int PageCount
        {
            get
            {
                int pageCount = TotalItemsCount / PageSize;
                int resto = TotalItemsCount % PageSize;

                if (resto > 0)
                    pageCount++;

                return pageCount;
            }
        }
    }
}