using Autocor.Catalogo.Web.Models;
using AutocorApi.Servicios.Core;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Controllers
{
    [Authorize]
    public class OfertasController : Controller
    {
        // --Servicios-- //

        private IServicioRubros _srvRubros;
        private IServicioProductos _srvProductos;
        //--------------//

        public OfertasController(IServicioRubros srvRubros, IServicioProductos srvProductos)
        {
            this._srvRubros = srvRubros;
            this._srvProductos = srvProductos;
        }

        // GET: Ofertas
        public ActionResult Index()
        {
            var rubrosOfertas = _srvRubros.ObtenerRubrosConOfertas();
            var model = new OfertasViewModel
            {
                Rubro = rubrosOfertas
            };
            return View(model);
        }

        [HttpGet]
        public JsonResult ObtenerProductos(int codigoRubro)
        {
            var productos = _srvProductos.ObtenerProductosEnOferta(codigoRubro);
            return Json(productos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerProductoSeleccionado(string codigoPieza)
        {
            var producto = _srvProductos.BuscarPorCodigo(codigoPieza);
            return Json(producto, JsonRequestBehavior.AllowGet);
        }
    }
}