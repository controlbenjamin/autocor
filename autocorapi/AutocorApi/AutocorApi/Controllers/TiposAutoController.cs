using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/tipos_auto")]
    public class TiposAutoController : ApiController
    {
        private IServicioTiposAuto servicioTiposAuto;

        public TiposAutoController(IServicioTiposAuto servicioTiposAuto)
        {
            this.servicioTiposAuto = servicioTiposAuto;
        }

        [ResponseType(typeof(TipoAutoDto))]
        [HttpGet, Route("{codigoTipoAuto:int}")]
        public IHttpActionResult Get(int codigoTipoauto)
        {
            var tipo = servicioTiposAuto.BuscarPorCodigo(codigoTipoauto);

            if (tipo == null)
                return NotFound();

            return Ok(tipo);
        }

        [ResponseType(typeof(IEnumerable<TipoAutoDto>))]
        [HttpGet, Route("")]
        public IHttpActionResult GetAll()
        {
            var tipos = servicioTiposAuto.ObtenerTiposAuto();
            return Ok(tipos);
        }
    }
}