using Autocor.Catalogo.Web.Extensions;
using Autocor.Catalogo.Web.Models;
using Autocor.Catalogo.Web.Utils.Mensaje;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Email;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Controllers
{
    public class ContactoController : Controller
    {
        private IServicioEmail _srvEmail;

        public ContactoController(IServicioEmail srvEmail)
        {
            this._srvEmail = srvEmail;       
        }


        // GET: Contacto
        public ActionResult Index()
        {
            ViewBag.Mensaje = TempData["mensaje"] as MensajeWeb;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactoLogin(ContactoModel contacto)
        {

            if (!ModelState.IsValid)
            {
                return View("Consulta");
            }

            EnviarContacto(contacto);
            this.SetMensajeView(new MensajeSimple("Su consulta ha sido enviada."));
            return View("Consulta");
        }

        public ActionResult VistaContacto()
        {
            return View("Consulta");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnviarEmail(ContactoModel contacto)
        {

            if (!ModelState.IsValid)
            {
                return View("Contacto");
            }

            EnviarContacto(contacto);
            TempData["mensaje"] = new MensajeSimple("Su consulta ha sido enviada.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private void EnviarContacto(ContactoModel contacto)
        {
            ConsultaDto consulta = new ConsultaDto();
            consulta.Nombre = contacto.NombreCliente;
            consulta.Email = contacto.EmailCliente;
            consulta.Mensaje = contacto.Mensaje;
            consulta.Asunto = contacto.Asunto;
            consulta.TelefonoCaracteristica = contacto.TelefonoCaracteristica;
            consulta.TelefonoNumero = contacto.TelefonoNumero;
            _srvEmail.EnviarEmailConsulta(consulta);
        }
    }
}

