using System;
using System.Collections.Generic;
using Autocor.Catalogo.Web.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutocorApi.Servicios.Core;
using Autocor.Catalogo.Web.Utils.Session;


namespace Autocor.Catalogo.Web.Controllers
{
    public class RegistroUsuarioWebController : Controller
    {
        private IServicioAutenticacion _srvAutenticacion;
        private IServicioClientes _srvClientes;
        private IServicioUsuariosWeb _srvUsuariosWeb;
     

        public RegistroUsuarioWebController(IServicioAutenticacion srvAutenticacion, IServicioClientes srvClientes, IServicioUsuariosWeb srvUsuariosWeb)
        {
            this._srvAutenticacion = srvAutenticacion;
            this._srvClientes = srvClientes;
            this._srvUsuariosWeb = srvUsuariosWeb;
        }


        // GET: RegistoUsuarioWeb
        [HttpGet]
        public ActionResult Index()
        {
            RegistroUsuarioWebModel usuario = new RegistroUsuarioWebModel { CodigoCliente = Convert.ToInt32(TempData["CodigoCliente"].ToString()) };
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(RegistroUsuarioWebModel usuario)
        {
            if (!ModelState.IsValid)
                return View("Index");

            if(SessionManager.Current.Usuario.Codigo != usuario.CodigoCliente) return RedirectToAction("Index", "Login");

            //TODO: ver que si un cliente ya tiene usuarioweb, no se pueda volver a registrar. 
            int codcli = usuario.CodigoCliente ?? default(int);
            var exito = _srvUsuariosWeb.NuevoUsuariosWeb(codcli,usuario.Email,usuario.Clave);
            if (!exito)
            {
                ModelState.AddModelError("Email", "Este mail ya esta en uso");
                return View("Index");
            }
            FormsAuthentication.SignOut();
            SessionManager.Current.Clear();
            return View("RegistroCompleto");
        }

    }
}