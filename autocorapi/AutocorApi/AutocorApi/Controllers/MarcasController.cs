using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/marcas")]
    public class MarcasController : ApiController
    {
        private IServicioMarcas servicioMarcas;

        public MarcasController(IServicioMarcas servicioMarcas)
        {
            this.servicioMarcas = servicioMarcas;
        }

        [ResponseType(typeof(IEnumerable<MarcaDto>))]
        [HttpGet, Route("{codigoMarca}")]
        public IHttpActionResult Get(string codigoMarca)
        {
            var marca = servicioMarcas.BuscarPorCodigo(codigoMarca);

            if (marca == null)
                return NotFound();

            return Ok(marca);
        }

        [ResponseType(typeof(IEnumerable<MarcaDto>))]
        [HttpGet, Route("")]
        public IHttpActionResult GetAll(int? rubro = null)
        {
            var marcas = servicioMarcas.ObtenerMarcas(rubro);
            return Ok(marcas);
        }
    }
}