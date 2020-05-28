using Autocor.Catalogo.Web.Extensions;
using Autocor.Catalogo.Web.Models;
using Autocor.Catalogo.Web.Utils.Mensaje;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Email;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Controllers
{
    public class RegistroController : Controller
    {
        private IServicioEmail _srvEmail;

        public RegistroController(IServicioEmail srvEmail)
        {
            this._srvEmail = srvEmail;
        }

        // GET: Registro
        public ActionResult Index()
        {
            ViewBag.Mensaje = TempData["mensaje"] as MensajeWeb;
            return View(new RegistroModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnviarEmail(RegistroModel registro)
        {
            if (!ModelState.IsValid)
            { return View("Index", registro); }

            RegistroDto registroDto = new RegistroDto();
            registroDto.Nombre = registro.Nombre;
            registroDto.Titular = registro.Titular;
            registroDto.CUIT = registro.CUIT;
            registroDto.TipoIVA = registro.TipoIVA;
            registroDto.IngresosBrutos = registro.IngresosBrutos;
            registroDto.DomicilioComercial = registro.DomicilioComercial;
            registroDto.CodigoPostal = registro.CodigoPostal;
            registroDto.Localidad = registro.Localidad;
            registroDto.Provincia = registro.Provincia;
            registroDto.Telefono = " ( " + registro.TelCaracteristica + " ) " + registro.Telefono;
            registroDto.Email = registro.Email;
            registroDto.Celular = " ( " + registro.CelCaracteristica + " ) " + registro.Celular;
            registroDto.MarcasComercializa = registro.MarcasComercializa;
            registroDto.RubrosComercializa = registro.RubrosComercializa;
            registroDto.FleteHabitual = registro.FleteHabitual;
            registroDto.FleteAlternativo = registro.FleteAlternativo;
            registroDto.Contactos = registro.Contactos;

            _srvEmail.EnviarEmailRegistro(registroDto);
            TempData["mensaje"] = new MensajeSimple("Su consulta ha sido enviada.");
            this.SetMensajeView(new MensajeSimple("Muchas gracias por registrarse. Nos pondremos en contacto a la brevedad para completar su alta como cliente."));

            return RedirectToAction("Index");
        }
    }
}