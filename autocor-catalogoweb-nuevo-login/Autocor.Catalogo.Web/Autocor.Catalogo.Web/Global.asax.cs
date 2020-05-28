using Autocor.Catalogo.Web.App_Start;
using AutocorApi.Servicios.Mappings;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Autocor.Catalogo.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        
            // AutoMap configuration
            MappingConfig.Initialize(NinjectWebCommon.Kernel);
        }
    }
}