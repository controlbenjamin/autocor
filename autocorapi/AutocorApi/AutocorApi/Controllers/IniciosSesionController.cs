
using System.Web.Http;
using System.Web.Http.Description;
using AutocorApi.Models.Filtros;
using AutocorApi.Models.Utils;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/iniciosSesion")]
    public class IniciosSesionController : ApiController
    {
        private IServicioIniciosSesion servicioIniciosSesion;

        public IniciosSesionController(IServicioIniciosSesion servicioIniciosSesion)
        {
            this.servicioIniciosSesion = servicioIniciosSesion;
        }

        [ResponseType(typeof(PagedResult<InicioSesionDto>))]
        [HttpGet]
        public IHttpActionResult Get([FromUri] FiltroInicioSesion filtro)
        {
            var iniciosSesion = servicioIniciosSesion.Buscar(
                codigoCliente: filtro.Cliente,
                tipoCatalogo: filtro.TipoCatalogo,
                fechaDesde: filtro.Desde,
                fechaHasta: filtro.Hasta,
                pagina: filtro.Page,
                tamanoPagina: filtro.Limit);

            var res = new PagedResult<InicioSesionDto>(iniciosSesion, "/api/iniciosSesion", filtro);
            return Ok(res);
        }

        [ResponseType(typeof(PagedResult<InicioSesionDto>))]
        [HttpGet, Route("cambiosUsuarioEscritorio")]
        public IHttpActionResult GetCambiosUsuarioEscritorio([FromUri] FiltroCambioUsuario filtro)
        {
            var iniciosSesion = servicioIniciosSesion.ObtenerCambiosUsuariosEscritorio(
                codigoCliente: filtro.Cliente,
                fechaDesde: filtro.Desde,
                fechaHasta: filtro.Hasta,
                pagina: filtro.Page,
                tamanoPagina: filtro.Limit);

            var res = new PagedResult<InicioSesionDto>(iniciosSesion, "/api/iniciosSesion/cambiosUsuarioEscritorio", filtro);
            return Ok(res);
        }
    }
}