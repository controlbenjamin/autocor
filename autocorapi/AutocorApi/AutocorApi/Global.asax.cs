using System.Web.Http;
using AutocorApi.App_Start;
using AutocorApi.Filters;
using AutocorApi.Servicios.Mappings;
using Newtonsoft.Json.Serialization;

namespace AutocorApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var kernel = NinjectWebCommon.Kernel;

            GlobalConfiguration.Configure(WebApiConfig.Register);

            // filtro global para las excepciones de servicio
            GlobalConfiguration.Configuration.Filters.Add(new ServiceExceptionFilter());

            // AutoMapper configuration
            MappingConfig.Initialize(kernel);

            // configuración del camelcase en los resultados de json
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
        }
    }
}