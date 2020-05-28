using Autocor.Catalogo.Web.Models;
using AutocorApi.Servicios.Core;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Controllers
{
    [Authorize]
    public class IncorporacionesController : Controller
    {
        // --Servicios-- //

        private IServicioRubros _srvRubros;
        private IServicioProductos _srvProductos;
        //--------------//

        public IncorporacionesController(IServicioRubros srvRubros, IServicioProductos srvProductos)
        {
            this._srvRubros = srvRubros;
            this._srvProductos = srvProductos;
        }

        // GET: Incorporaciones
        public ActionResult Index()
        {
            var rubrosIncorporaciones = _srvRubros.ObtenerRubrosConIncorporaciones();
            var model = new IncorporacionesViewModel
            {
                Rubro = rubrosIncorporaciones
            };
            return View(model);
        }

        [HttpGet]
        public JsonResult ObtenerProductos(int codigoRubro)
        {
            var productos = _srvProductos.ObtenerProductosIncorporados(codigoRubro);
            return Json(productos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerProductoSeleccionado(string codigoPieza)
        {
            var producto = _srvProductos.BuscarPorCodigo(codigoPieza);
            return Json(producto, JsonRequestBehavior.AllowGet);
        }
    }
}