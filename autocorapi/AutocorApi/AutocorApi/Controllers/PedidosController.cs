using System.Web.Http;
using System.Web.Http.Description;
using AutocorApi.Extensions;
using AutocorApi.Models.Filtros;
using AutocorApi.Models.Utils;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Core.Pedidos;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Pedidos;

namespace AutocorApi.Controllers
{
    [Authorize]
    //[ServiceExceptionFilter]
    [RoutePrefix("api/pedidos")]
    public class PedidosController : ApiController
    {
        private IServicioConsultaPedidos servicioConsultaPedidos;
        private IServicioPedidos servicioPedidos;

        public PedidosController(IServicioConsultaPedidos servicioConsultaPedidos, IServicioPedidos servicioPedidos)
        {
            this.servicioConsultaPedidos = servicioConsultaPedidos;
            this.servicioPedidos = servicioPedidos;
        }

        [ResponseType(typeof(PedidoDto))]
        [HttpGet, Route("{id}")]
        public IHttpActionResult GetPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Número de pedido no válido");
            }

            var claims = this.GetClaimsUsuario();
            var pedido = servicioConsultaPedidos.BuscarPorId(id);

            switch (claims.Rol)
            {
                case Rol.CLIENTE:
                    
                    // el pedido tiene que ser del cliente (no tiene que ver pedidos de otro cliente)
                    if(pedido.CodigoCliente != claims.CodigoCliente)
                    {
                        // el pedido no es del cliente
                        pedido = null;
                    }

                    break;
                case Rol.VIAJANTE:

                    // el pedido tiene que ser de un cliente que sea de su zona
                    int zona = claims.ZonaViajante;

                    if(pedido.Cliente.CodigoZona != zona)
                    {
                        pedido = null;
                    }

                    break;
                default:
                    break;
            }

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [ResponseType(typeof(PagedResult<PedidoDto>))]
        [HttpGet]
        public IHttpActionResult Get([FromUri] FiltroPedido filtro)
        {
            // limitar pedidos por cliente (no puede ver los pedidos de otros)

            var claims = this.GetClaimsUsuario();

            switch(claims.Rol)
            {
                case Rol.CLIENTE:
                    filtro.Cliente = claims.CodigoCliente; ; // se agrega al filtro el cliente según el token
                    break;
                case Rol.VIAJANTE:
                    filtro.ZonaCliente = claims.ZonaViajante;
                    break;
                case Rol.ADMIN:
                default:
                    break;
            }
            
            var pedidos = servicioConsultaPedidos.Buscar(
                codigoCliente: filtro.Cliente,
                fechaDesde: filtro.Desde,
                fechaHasta: filtro.Hasta,
                zonaCliente:filtro.ZonaCliente,
                idEstado: filtro.Estado,
                pagina: filtro.Page,
                tamanoPagina: filtro.Limit);

            var res = new PagedResult<PedidoDto>(pedidos, "/api/pedidos", filtro);
            return Ok(res);
        }

        [HttpPost]
        public IHttpActionResult Post(NuevoPedidoDto pedido)
        {
            if (pedido == null)
            {
                return BadRequest("Pedido no válido");
            }

            var pedidoGenerado = servicioPedidos.GuardarPedido(pedido);
            return Created($"api/pedidos/{pedidoGenerado.Id}", pedidoGenerado);
        }
    }
}