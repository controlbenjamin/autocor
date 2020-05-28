using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocorApi.Repositorios.Utils
{
    public class PageConfig
    {
        private PageConfig(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int Offset => (PageNumber - 1) * PageSize;

        public static PageConfig Create(int pageNumber, int pageSize)
        {
            return new PageConfig(pageNumber, pageSize);
        }
    }
}
