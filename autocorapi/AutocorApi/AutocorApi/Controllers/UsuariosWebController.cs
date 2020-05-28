using AutocorApi.Models.Utils;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/usuariosweb")]
    public class UsuariosWebController : ApiController
    {
        private readonly IServicioUsuariosWeb _servicioUsuariosWeb;

        public UsuariosWebController(IServicioUsuariosWeb servicioUsuariosWeb)
        {
            _servicioUsuariosWeb = servicioUsuariosWeb;
        }

        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(IEnumerable<UsuarioWebDto>))]
        [HttpGet, Route("{pagina:int}/{tamPagina:int}")]
        public IHttpActionResult GetUsuarios(int pagina, int tamPagina)
        {
            var usuarios = _servicioUsuariosWeb.ObtenerUsuariosWeb(pagina, tamPagina);
            return Ok(usuarios);
        }

        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(bool?))]
        [HttpPost, Route("actualizarestado")]
        public IHttpActionResult ActualizarEstado([FromBody] ActualizacionEstado nuevoEstado)
        {
            //reusando actualizacionEstado; instalacion = nrocliente en este caso
            bool? respuesta = null;
            try
            {
                _servicioUsuariosWeb.ActualizarEstado(nuevoEstado.instalacion, nuevoEstado.estado);
                respuesta = nuevoEstado.estado;
            }
            catch (Exception) { }

            return Ok(respuesta);
        }

        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(PagedResult<UsuarioWebDto>))]
        [HttpGet, Route("buscar")]
        public IHttpActionResult BuscarUsuarios(int pagina = 1, int tamPagina = 5, string criterio = "", DateTime? fecha = null)
        {
            //var usuario = System.Security.Principal.WindowsIdentity.GetCurrent().Token.ToString();
            var usuarios = _servicioUsuariosWeb.BuscarUsuariosWeb(pagina, tamPagina, criterio, fecha);
            return Ok(usuarios);
        }

        [Authorize(Roles = "ADMIN")]
        //[ResponseType(typeof(bool?))]
        [HttpPost, Route("eliminar")] //TODO: cambiar por delete
        public IHttpActionResult EliminarUsuario([FromBody]EliminarUsuarioWeb usuario)
        {
            var resultado = _servicioUsuariosWeb.BorrarUsuarioWeb(usuario.codcli);
            if (!resultado) return BadRequest();
            return Ok();
        }

    }
}
