using Autocor.Catalogo.Web.Models.Errores;
using Autocor.Catalogo.Web.Utils;
using Autocor.Catalogo.Web.Utils.Mensaje;
using System;
using System.Web;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Filters
{
    public class BaseErrorHandler : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
                return;

            int statusCode = 500;   // petición errónea
            string message = filterContext.Exception.Message;
            string detail = null;
            Exception exception = filterContext.Exception;

            if (exception is HttpException)
            {
                statusCode = (exception as HttpException).GetHttpCode();
                detail = message;
                message = MensajesErrorHttp.GetError(statusCode);
            }
            else
            {
                message = "Disculpe, ha ocurrido un problema.";
            }

            // si la petición es Ajax, retorna un JSON en vez de una vista
            if (IsAjax(filterContext))
            {
                filterContext.Result = new JsonResult()
                {
                    Data = new MensajeDataError(message),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                // excepción normal
                // base.OnException(filterContext);
                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary<ErrorModel>(new ErrorModel(exception))
                };
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = statusCode; // dragon server cannont set status after HTTP headers has been set

            // Logging

            //if want to get different of the request
            string controller = (string)filterContext.RouteData.Values["controller"];
            string action = (string)filterContext.RouteData.Values["action"];
        }

        private bool IsAjax(ExceptionContext filterContext)
        {
            // return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return filterContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}