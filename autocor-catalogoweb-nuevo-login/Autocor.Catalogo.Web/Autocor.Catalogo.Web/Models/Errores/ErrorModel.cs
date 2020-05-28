using Autocor.Catalogo.Web.Utils;
using Autocor.Catalogo.Web.Utils.Mensaje;
using System;
using System.Net;
using System.Web;

namespace Autocor.Catalogo.Web.Models.Errores
{
    public class ErrorModel
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public ErrorModel()
        {
            Titulo = "Error";
            Mensaje = "Se ha producido un error al procesar su petición";
            Detalle = null;
        }

        public ErrorModel(string mensaje, string titulo = null, string detalle = null)
        {
            Titulo = titulo ?? "Error";
            Mensaje = mensaje;
            Detalle = detalle;
        }

        public ErrorModel(Exception ex, string titulo = null)
        {
            Titulo = titulo ?? "Error";
            Mensaje = ex.Message;

            if (ex.InnerException != null)
            {
                Detalle = ex.InnerException.Message;
            }
        }
    }

    public class ErrorModelHttp : ErrorModel
    {
        public HttpException Exception { get; protected set; }

        public ErrorModelHttp(HttpException ex)
        {
            Titulo = MensajesErrorHttp.GetError(ex.GetHttpCode());
            Mensaje = ex.Message;

            if (ex.InnerException != null)
            {
                Detalle = ex.InnerException.Message;
            }

            Exception = ex;
        }

        public ErrorModelHttp(HttpStatusCode httpStatusCode, string mensaje = null)
        {
            int code = (int)httpStatusCode;
            string titulo = MensajesErrorHttp.GetError(code);

            Titulo = titulo;
            Mensaje = mensaje;
        }
    }
}