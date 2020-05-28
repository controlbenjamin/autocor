using Autocor.Catalogo.Web.Extensions;
using Autocor.Catalogo.Web.Models;
using Autocor.Catalogo.Web.Utils.Mensaje;
using Autocor.Catalogo.Web.Utils.Session;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Core.Pedidos;
using AutocorApi.Servicios.Dto.Carrito;
using AutocorApi.Servicios.Dto.Pedidos;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Controllers
{
    [Authorize]
    public class CarritoController : Controller
    {
        //  Servicios  //
        private IServicioCarrito _srvCarrito;
        private IServicioPedidos _srvPedido;

        public CarritoController(IServicioCarrito srvCarrito,IServicioPedidos srvPedido, IServicioUsuariosWeb srvUsuariosWeb)
        {
            this._srvCarrito = srvCarrito;
            this._srvPedido = srvPedido;
        }

        // GET: ItemCarrito
        public ActionResult Index()
        {
            var cliente = SessionManager.Current.Usuario.Codigo;
            
            var carritoResult = _srvCarrito.ObtenerCarritoPorCliente(cliente);

            var model = new ItemsCarritoViewModel
            {
                Items = carritoResult
            };
          
            this.SetMensajeViewFromTemp();
     
            return View(model);
        }

        [HttpGet]
        public ActionResult ObtenerCarrito()
        {
            var cliente = SessionManager.Current.Usuario.Codigo;
            var carritoResult = _srvCarrito.ObtenerCarritoPorCliente(cliente);
            return Json(carritoResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarCarrito(string codigoPieza, int cantidad, bool acumularCantidad)
        {
            var codigoCliente = SessionManager.Current.Usuario.Codigo;

            EditItemCarritoDto itemCarrito = new EditItemCarritoDto
            {
                CodigoPieza = codigoPieza,
                Cantidad = cantidad,
                CodigoCliente = codigoCliente
            };

            _srvCarrito.GuardarItemCarrito(itemCarrito, acumularCantidad: acumularCantidad);

            var mensaje = new MensajeSimple("Producto añadido al carrito");
            return Json(mensaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarItemCarrito(string codigoPieza)
        {
            var codigoCliente = SessionManager.Current.Usuario.Codigo;

            _srvCarrito.EliminarItemCarrito(codigoCliente, codigoPieza);

            var mensaje = new MensajeSimple("Se eliminó el producto del carrito");
            return Json(mensaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VaciarCarrito()
        {
            var codigoCliente = SessionManager.Current.Usuario.Codigo;
            _srvCarrito.VaciarCarrito(codigoCliente);
            var mensaje = new MensajeSimple("Producto añadido al carrito");
            return Json(mensaje);
        }

        [HttpPost]
        public ActionResult InsertarPedido(string observaciones,decimal total)
        {
            if(string.IsNullOrEmpty(observaciones))
            {
                observaciones = string.Empty;
            }

            var cliente = SessionManager.Current.Usuario.Codigo;
            var itemCarrito = _srvCarrito.ObtenerCarritoPorCliente(cliente);

            var listaDetalle = new List<NuevoDetallePedidoDto>();

            foreach (var item in itemCarrito)
            {
                listaDetalle.Add(new NuevoDetallePedidoDto
                {
                   Cantidad = item.Cantidad,
                   CodigoPieza = item.CodigoPieza,
                   PrecioUnitario = item.Producto.Precio
                });
            }

            var pedido = new NuevoPedidoDto
                {
                    CodigoCliente = cliente,
                    Observaciones = observaciones,
                    Detalles = listaDetalle
                };
    
           _srvPedido.GuardarPedido(pedido);

            _srvCarrito.VaciarCarrito(cliente);

            this.SetMensajeTemp(new MensajeSimple("Pedido enviado exitosamente"));

            return RedirectToAction("Index");
        }
     
    }
}