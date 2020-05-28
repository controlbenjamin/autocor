using Autocor.Catalogo.Web.Models;
using AutocorApi.Servicios.Core;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Controllers
{
    [Authorize]
    public class DescargasController : Controller
    {
        private IServicioActualizaciones _srvActualizaciones;

        public DescargasController(IServicioActualizaciones srvActualizaciones) 
        {
            this._srvActualizaciones = srvActualizaciones;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //string path = Server.MapPath("~")+ "\\Autocor\\preciosv4.exe";

     
             var actualizacion = new ActualizacionesModel
            {
                FechaUltimaActualizacion = _srvActualizaciones.ObtenerUltimaActualizacion().Fecha.Value
                //FechaUltimaActualizacionV4 = System.IO.File.GetLastWriteTime(path)
            };

            return View(actualizacion);
        }
    }
}