using AutocorApi.Extensions;
using AutocorApi.Models.Filtros;
using AutocorApi.Servicios.Dto.Utils;

namespace AutocorApi.Models.Utils
{
    public class PagedResult<T> : PagedResultDto<T> where T : class
    {
        public PagedResult(PagedResultDto<T> pagedResult)
            : base(pagedResult.Items, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalItemsCount)
        {
        }

        public PagedResult(PagedResultDto<T> pagedResult, string baseUrl, FiltroPaginacion filtroPaginacion)
            : base(pagedResult.Items, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalItemsCount)
        {
            SetPagtionationUrls(baseUrl, filtroPaginacion);
        }

        public string UrlNext { get; set; }
        public string UrlPrevious { get; set; }

        private void SetPagtionationUrls(string baseUrl, FiltroPaginacion filtroPaginacion)
        {
            if (this.PageNumber > 1)
            {
                filtroPaginacion.Page--;
                this.UrlPrevious = string.Format("{0}?{1}", baseUrl, filtroPaginacion.ToQueryString());
                filtroPaginacion.Page++;
            }

            if (this.PageNumber < this.PageCount)
            {
                filtroPaginacion.Page++;
                this.UrlNext = string.Format("{0}?{1}", baseUrl, filtroPaginacion.ToQueryString());
                filtroPaginacion.Page--;
            }
        }
    }
}