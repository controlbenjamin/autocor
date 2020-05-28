using Autocor.Catalogo.Web.Models;
using Autocor.Catalogo.Web.Utils.Session;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Controllers
{
    [Authorize]
    public class MiCuentaController : Controller
    {
        private IServicioClientes _srvCliente;

        public MiCuentaController(IServicioClientes srvCliente)
        {
            this._srvCliente = srvCliente;
        }

        // GET: MiCuenta
        public ActionResult Index()
        {
            var model = new MiCuentaModel
            {
                NroCliente = SessionManager.Current.Usuario.Codigo,
                Cuit = SessionManager.Current.Usuario.CUIT,
                Titular = SessionManager.Current.Usuario.RazonSocial
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult ObtenerConfiguracionCliente()
        {
            var configuracion = SessionManager.Current.Usuario.Configuracion;

            return Json(new
            {
                beneficio = configuracion.Beneficio,
                descuento = configuracion.Descuento,
                iva = configuracion.IVA
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActualizarConfiguracion(ConfiguracionClienteDto configuracion)
        {
            configuracion.CodigoCliente = SessionManager.Current.Usuario.Codigo;
            _srvCliente.GuardarConfiguracion(configuracion);
            return Json(configuracion);
        }
    }
}