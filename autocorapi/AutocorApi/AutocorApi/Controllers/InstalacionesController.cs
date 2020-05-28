using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutocorApi.Models.Utils;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/instalaciones")]
    public class InstalacionesController : ApiController
    {
        //TODO: cambiar a modo rest - usar put, get, post. 
        //TODO: modificar nombres
        //TODO: cambiar entradas a objeto.
        //TODO: cambiar a servicios?
        private IServicioInstalaciones _servicioInstalaciones;

        public InstalacionesController(IServicioInstalaciones servicioInstalaciones)
        {
            _servicioInstalaciones = servicioInstalaciones;
        }

        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(IEnumerable<InstalacionDto>))]
        [HttpGet, Route("{pagina:int}/{tamPagina:int}")] //preguntar si get o post, posiblemente cambiar  apost
        public IHttpActionResult GetInstalaciones(int pagina, int tamPagina)
        {
            var instalaciones = _servicioInstalaciones.ObtenerInstalaciones(pagina, tamPagina);
            return Ok(instalaciones);
        }

        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(int))]
        [HttpGet, Route("paginas/{tamPagina:int}")] 
        public IHttpActionResult GetPaginas(int tamPagina)
        {
            //borrar
            //var paginas = _servicioInstalaciones.ObtenerCantidadPagina(tamPagina);
            return Ok(0);
        }

        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(bool?))]
        [HttpPost, Route("actualizarestado")]
        public IHttpActionResult ActualizarEstado([FromBody] ActualizacionEstado nuevoEstado)
        {
            bool? respuesta = null;
            try
            {
                _servicioInstalaciones.ActualizarEstado(nuevoEstado.instalacion, nuevoEstado.estado);
                respuesta = nuevoEstado.estado;
            }
            catch (Exception) { }
            
            return Ok(respuesta);
        }

        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(PagedResult<InstalacionDto>))]
        [HttpGet, Route("buscar")]
        public IHttpActionResult BuscarInstalaciones(int pagina=1, int tamPagina=5, string criterio="", DateTime? fecha= null)
        {
            var instalaciones = _servicioInstalaciones.BuscarInstalaciones(pagina,tamPagina,criterio,fecha);
            return Ok(instalaciones);
        }

        [ResponseType(typeof(PagedResult<InstalacionDto>))]
        [HttpGet, Route("NuevaInstalacion")]
        public IHttpActionResult NuevaInstalacion(int CodigoCliente, string email, string nombrePC)
        {
            var instalacion = _servicioInstalaciones.NuevaInstalacion(CodigoCliente,email,nombrePC);
            return Ok(instalacion);
        }
    }
}