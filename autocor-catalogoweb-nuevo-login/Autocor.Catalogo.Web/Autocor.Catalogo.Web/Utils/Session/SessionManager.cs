using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto.Clientes;
using System.Web;
using System.Web.SessionState;

namespace Autocor.Catalogo.Web.Utils.Session
{
    public class SessionManager
    {
        private HttpSessionState _session;
        private IServicioAutenticacion _srvAutenticacion;

        private SessionManager(ClienteDto usuario)
        {
            this.Usuario = usuario;
            this._session = HttpContext.Current.Session;
        }

        public static void Inicializar(ClienteDto usuario)
        {
            SessionManager sesMgr = new SessionManager(usuario);
            HttpContext.Current.Session.Timeout = 525600;
            HttpContext.Current.Session[SessionConstants.SESSION_MANAGER] = sesMgr;
        }

        public static SessionManager Current
        {
            get
            {
                var sesMgr = HttpContext.Current.Session[SessionConstants.SESSION_MANAGER] as SessionManager;

                if (sesMgr == null)
                {
                    HttpContext.Current.Session.Abandon();
                    HttpContext.Current.Response.Redirect("~/Login");
                    HttpContext.Current.Response.End();

                    // throw new HttpException((int)HttpStatusCode.Forbidden, "Debe iniciar sesión");
                }

                return sesMgr;
            }
        }

        public ClienteDto Usuario { get; }

        private void Update()
        {
            HttpContext.Current.Session[SessionConstants.SESSION_MANAGER] = this;
        }

        public void Clear()
        {
            _session.Abandon();
        }
    }
}