using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AutocorApi.Filters;
using AutocorApi.Models.Carrito;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto.Carrito;

namespace AutocorApi.Controllers
{
    [Authorize]
    [ServiceExceptionFilter]
    [RoutePrefix("api/carrito")]
    public class CarritoController : ApiController
    {
        private IServicioCarrito servicioCarrito;

        public CarritoController(IServicioCarrito servicioCarrito)
        {
            this.servicioCarrito = servicioCarrito;
        }

        [ResponseType(typeof(IEnumerable<ItemCarritoDto>))]
        [HttpGet, Route("{codigoCliente:int}")]
        public IHttpActionResult Get(int codigoCliente)
        {
            if (codigoCliente < 0)
                return BadRequest("Cliente no válido");

            var carrito = servicioCarrito.ObtenerCarritoPorCliente(codigoCliente);

            return Ok(carrito);
        }

        [HttpPost]
        public IHttpActionResult Post(CarritoClienteModel carrito)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var itemsCarrito = carrito.Items.Select(x => new EditItemCarritoDto
            {
                CodigoCliente = carrito.CodigoCliente,
                CodigoPieza = x.CodigoPieza,
                Cantidad = x.Cantidad
            });

            servicioCarrito.GuardarItemsCarrito(carrito.CodigoCliente, itemsCarrito);
            return Ok();
        }

        [HttpDelete, Route("{codigoCliente:int}/{codigoPieza}")]
        public IHttpActionResult Delete(int codigoCliente, string codigoPieza)
        {
            servicioCarrito.EliminarItemCarrito(codigoCliente, codigoPieza);
            return Ok();
        }

        [HttpDelete, Route("{codigoCliente:int}")]
        public IHttpActionResult VaciarCarrito(int codigoCliente)
        {
            servicioCarrito.VaciarCarrito(codigoCliente);
            return Ok();
        }
    }
}