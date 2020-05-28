using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutocorApi.Extensions;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/rubros")]
    public class RubrosController : ApiController
    {
        private IServicioRubros servicioRubros;

        public RubrosController(IServicioRubros servicioRubros)
        {
            this.servicioRubros = servicioRubros;
        }

        [ResponseType(typeof(RubroDto))]
        [HttpGet, Route("{codigoRubro:int}")]
        public IHttpActionResult Get(int codigoRubro)
        {
            var res = servicioRubros.BuscarPorCodigo(codigoRubro);

            if (res == null)
                return NotFound();

            return Ok(res);
        }

        [ResponseType(typeof(IEnumerable<RubroDto>))]
        [HttpGet, Route("")]
        public IHttpActionResult GetAll()
        {
            var rubros = servicioRubros.ObtenerRubros();
            return Ok(rubros);
        }
    }
}