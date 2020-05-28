using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/actualizaciones")]
    public class ActualizacionesController : ApiController
    {
        private IServicioActualizaciones servicioActualizaciones;

        public ActualizacionesController(IServicioActualizaciones servicioActualizaciones)
        {
            this.servicioActualizaciones = servicioActualizaciones;
        }

        [ResponseType(typeof(ActualizacionDto))]
        [HttpGet, Route("ultima")]
        public IHttpActionResult GetUltima()
        {
            var actualizacion = servicioActualizaciones.ObtenerUltimaActualizacion();

            if (actualizacion == null)
                return NotFound();

            return Ok(actualizacion);
        }

        [ResponseType(typeof(IEnumerable<ActualizacionDto>))]
        [HttpGet, Route("ultimas/{cantidad:int}")]
        public IHttpActionResult GetUltimas(int cantidad = 5)
        {
            var actualizaciones = servicioActualizaciones.ObtenerUltimasActualizaciones(cantidad);
            return Ok(actualizaciones);
        }
    }
}