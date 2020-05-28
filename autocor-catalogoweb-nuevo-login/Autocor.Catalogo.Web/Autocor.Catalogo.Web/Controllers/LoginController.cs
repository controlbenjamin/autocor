using Autocor.Catalogo.Web.Filters;
using Autocor.Catalogo.Web.Models;
using Autocor.Catalogo.Web.Utils.Session;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Autocor.Catalogo.Web.Controllers
{
    [BaseErrorHandler]
    public class LoginController : Controller
    {
        private IServicioAutenticacion _srvAutenticacion;
        private IServicioClientes _srvClientes;

        public LoginController(IServicioAutenticacion srvAutenticacion, IServicioClientes srvClientes)
        {
            this._srvAutenticacion = srvAutenticacion;
            this._srvClientes = srvClientes;
        }

        // GET: Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if(authCookie == null) return View();
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            var cliente = _srvClientes.BuscarPorUsuario(ticket.Name);
            SessionManager.Inicializar(cliente);

            return RedirectToAction("Index", "Catalogo");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IniciarSesion(UsuarioModel usuarioModel)
        {
            if (!ModelState.IsValid)
                return View("Index", usuarioModel);

            var usuario = _srvAutenticacion.ValidarUsuarioWeb(usuarioModel.Nombre, usuarioModel.Password);

            if (usuario == null || usuario.EstadoWeb == EstadoWeb.USUARIO_WEB) // "si el nombre y pass son incorrectos, o si se usa nombre y cuil cuando ya esta registrado un mail y pass"
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View("Index", usuarioModel);
            }

            //benja --> se quita que por no estar autorizado el usuario vuelva a login
            //if (usuario.Estado == null && usuario.Rol == Rol.CLIENTE && usuario.EstadoWeb != EstadoWeb.USUARIO_SIN_USUARIO_WEB)
            //{
            //    ModelState.AddModelError("", "Usuario pendiente de autorizacion.");
            //    return View("Index", usuarioModel);
            //}

            if (usuario.Estado == false)
            {
                ModelState.AddModelError("", "Usuario bloqueado.");
                return View("Index", usuarioModel);
            }

            if (usuario.Rol != Rol.CLIENTE)
            {
                ModelState.AddModelError("", "Usuario no válido");
                return View("Index", usuarioModel);
            }

            if (!usuario.Activo)
            {
                ModelState.AddModelError("", "Usuario desactivado");
                return View("Index", usuarioModel);
            }

            var cliente = _srvClientes.BuscarPorUsuario(usuario.NombreUsuario);
            SessionManager.Inicializar(cliente);

            string ip = Request.UserHostAddress;

            _srvAutenticacion.RegistrarInicioSesion(new InicioSesionDto
            {
                CodigoCliente = cliente.Codigo,
                CodigoClienteAnterior = null,
                Email = null,
                Fecha = DateTime.Now,
                NombrePC = null,
                UsuarioPC = null,
                TipoCatalogo = InicioSesionDto.TipoCatalogo_Web
            });

            FormsAuthentication.SetAuthCookie(usuario.NombreUsuario, usuarioModel.MantenerSesion); // original: usuarioModel.Nombre, false
            if (usuario.EstadoWeb == EstadoWeb.USUARIO_SIN_USUARIO_WEB)
            {
                TempData["CodigoCliente"] = usuario.CodigoCliente;
                return RedirectToAction("Index", "RegistroUsuarioWeb");
            }
            return RedirectToAction("Index", "Catalogo");
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            SessionManager.Current.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}