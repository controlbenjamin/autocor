using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AutocorApi.Extensions;
using AutocorApi.Models.Clientes;
using AutocorApi.Models.Filtros;
using AutocorApi.Models.Utils;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Clientes;
using AutocorApi.Servicios.Dto.Utils;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/clientes")]
    public class ClientesController : ApiController
    {
        private IServicioClientes servicioClientes;

        public ClientesController(IServicioClientes srvClientes)
        {
            this.servicioClientes = srvClientes;
        }

        [ResponseType(typeof(ClienteDto))]
        [HttpGet, Route("cuit/{cuit}")]
        public IHttpActionResult Cuit(string cuit)
        {
            if (string.IsNullOrEmpty(cuit))
            {
                return BadRequest("No especificó CUIT");
            }

            var cliente = servicioClientes.BuscarPorCuit(cuit);

            if (cliente == null)
            {
                return NotFound();
            }

            if(this.IsInRol(Rol.CLIENTE))
            {
                var claims = this.GetClaimsUsuario();

                if(cliente.Codigo != claims.CodigoCliente)
                {
                    return NotFound();
                }
            }

            return Ok(cliente);
        }

        [ResponseType(typeof(ClienteDto))]
        [HttpGet, Route("{codigoCliente:int}")]
        public IHttpActionResult Numero(int codigoCliente)
        {
            var cliente = servicioClientes.BuscarPorNumero(codigoCliente);

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        [Authorize(Roles = "ADMIN,VIAJANTE")]
        [ResponseType(typeof(ResultadoBusquedaClientesModel))]
        [HttpGet, Route("q/{busqueda}")]
        public IHttpActionResult Buscar(string busqueda)
        {
            if (string.IsNullOrEmpty(busqueda))
                return BadRequest("No se especificó criterio de búsqueda");

           ClienteDto auxCliente = null;
            int? zona = null;

            if(this.IsInRol(Rol.VIAJANTE))
            {
                // buscar la zona 
                var claims = this.GetClaimsUsuario();
                zona = claims.ZonaViajante;

                if (zona < 0)
                {
                    return InternalServerError(); // el vendedor no tiene zona válida
                }
            }

            if (int.TryParse(busqueda, out int codigoCliente))
            {
                auxCliente = servicioClientes.BuscarPorNumero(codigoCliente);

                // si se busca por zona y no coincide con la del cliente, se pone en nulo
                if(zona.HasValue && auxCliente.CodigoZona != zona.Value)
                {
                    auxCliente = null;
                }
            }

            var clientes = servicioClientes.BuscarPorRazonSocial(busqueda, zona: zona).ToList();

            return Ok(new ResultadoBusquedaClientesModel
            {
                PorCodigo = auxCliente,
                PorRazonSocial = clientes
            });
        }

        [Authorize(Roles = "ADMIN,VIAJANTE")]
        [ResponseType(typeof(PagedResultDto<ClienteMinDto>))]
        [HttpGet, Route("listado")]
        public IHttpActionResult Listado([FromUri]FiltroListadoClientes filtro)
        { 
            if(filtro == null)
            {
                filtro = new FiltroListadoClientes();
            }

            if (this.IsInRol(Rol.VIAJANTE))
            {
                // buscar la zona 
                var claims = this.GetClaimsUsuario();
                filtro.Zona = claims.ZonaViajante;

                if (filtro.Zona < 0)
                {
                    return InternalServerError(); // el vendedor no tiene zona válida
                }
            }

            var clientes = servicioClientes.ObtenerListado(zona: filtro.Zona, pagina: filtro.Page, tamanoPagina: filtro.Limit);
            return Ok(new PagedResult<ClienteMinDto>(clientes, "/api/clientes/listado", filtro));
        }

        
    }
}