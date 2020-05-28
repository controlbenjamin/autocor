using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/descargas")]
    public class DescargasController : ApiController
    {
        private IServicioDescargas servicioDescargas;

        public DescargasController(IServicioDescargas servicioDescargas)
        {
            this.servicioDescargas = servicioDescargas;
        }

        [ResponseType(typeof(IEnumerable<DescargaDto>))]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var descargas = servicioDescargas.ObtenerDescargas();
            return Ok(descargas);
        }
    }
}