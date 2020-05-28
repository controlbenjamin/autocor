using Autocor.Catalogo.Web.Filters;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new BaseErrorHandler());
        }
    }
}
