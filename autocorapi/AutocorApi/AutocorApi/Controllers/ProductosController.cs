using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutocorApi.Models.Filtros;
using AutocorApi.Models.Utils;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Dto.Productos;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/productos")]
    public class ProductosController : ApiController
    {
        private IServicioProductos servicioProductos;

        public ProductosController(IServicioProductos servicioProductos)
        {
            this.servicioProductos = servicioProductos;
        }

        [ResponseType(typeof(ProductoDto))]
        [HttpGet, Route("{codigoPieza}")]
        public IHttpActionResult Get(string codigoPieza)
        {
            if (string.IsNullOrEmpty(codigoPieza))
                return BadRequest("Código de pieza no especificado");

            var res = servicioProductos.BuscarPorCodigo(codigoPieza);

            if (res == null)
                return NotFound();

            return Ok(res);
        }

        [ResponseType(typeof(PagedResult<ProductoDto>))]
        [HttpGet, Route("")]
        public IHttpActionResult GetAll([FromUri]FiltroProducto filtro)
        {
            if (filtro == null)
                filtro = new FiltroProducto();

            var productos = servicioProductos.ObtenerProductos(filtro.Descripcion, filtro.Rubro, filtro.Marca, filtro.TipoAuto,
                pagina: filtro.Page, tamanoPagina: filtro.Limit);

            System.Diagnostics.Debug.WriteLine(productos.TotalItemsCount);

            var res = new PagedResult<ProductoDto>(productos, "/api/productos", filtro);

            

            return Ok(res);
        }

        [ResponseType(typeof(IEnumerable<ProductoDto>))]
        [HttpGet, Route("{codigoPieza}/equivalencias")]
        public IHttpActionResult Equivalencias(string codigoPieza, bool incluirOrigen = false)
        {
            if (string.IsNullOrEmpty(codigoPieza))
                return BadRequest("Codigo de pieza no especificado");

            var productos = servicioProductos.ObtenerProductosEquivalentes(codigoPieza, incluirOrigen);
            return Ok(productos);
        }

        [ResponseType(typeof(IEnumerable<ParametroProductoDto>))]
        [HttpGet, Route("{codigoPieza}/parametros")]
        public IHttpActionResult Parametros(string codigoPieza)
        {
            if (string.IsNullOrEmpty(codigoPieza))
                return BadRequest("Codigo de pieza no especificado");

            var parametros = servicioProductos.ObtenerParametros(codigoPieza);
            return Ok(parametros);
        }

        [ResponseType(typeof(StockDto))]
        [HttpGet, Route("{codigoPieza}/stock")]
        public IHttpActionResult Stock(string codigoPieza)
        {
            if (string.IsNullOrEmpty(codigoPieza))
                return BadRequest("Codigo de pieza no especificado");

            var stock = servicioProductos.ObtenerStockActual(codigoPieza);

            if (stock.StockReal < 0)
                return NotFound();

            return Ok(stock);
        }

        [ResponseType(typeof(IEnumerable<ProductoDto>))]
        [HttpGet, Route("incorporaciones/{codigoRubro:int}")]
        public IHttpActionResult Incorporaciones(int codigoRubro)
        {
            if (codigoRubro <= 0)
                return BadRequest("Rubro no válido");

            var incorporaciones = servicioProductos.ObtenerProductosIncorporados(codigoRubro);
            return Ok(incorporaciones);
        }

        [ResponseType(typeof(IEnumerable<ProductoDto>))]
        [HttpGet, Route("ofertas/{codigoRubro:int}")]
        public IHttpActionResult Ofertas(int codigoRubro)
        {
            if (codigoRubro <= 0)
                return BadRequest("Rubro no válido");

            var ofertas = servicioProductos.ObtenerProductosEnOferta(codigoRubro);
            return Ok(ofertas);
        }

        [ResponseType(typeof(IEnumerable<ProductoDto>))]
        [HttpGet, Route("random/{cantidad:int}")]
        public IHttpActionResult Random(int cantidad = 10)
        {
            var randoms = servicioProductos.ObtenerRandoms(cantidad);
            return Ok(randoms);
        }

        [ResponseType(typeof(IEnumerable<ProductoDto>))]
        [HttpGet, Route("masPedidos/{cantidad:int}")]
        public IHttpActionResult MasPedidos(int cantidad = 10)
        {
            var masPedidos = servicioProductos.ObtenerMasPedidosEnMes(cantidad: cantidad);
            return Ok(masPedidos);
        }
    }
}