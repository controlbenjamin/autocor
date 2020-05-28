using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace AutocorApi.Filters
{
    /// <summary>
    /// Convierte valores <c>null</c> en una respuesta HTTP con código 404 (Not Found)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NullToNotFound : ActionFilterAttribute
    {
        /**
         * Referencia: http://stackoverflow.com/questions/12592456/asp-net-web-api-and-status-code-for-null-response
         * */

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null && actionExecutedContext.Response.IsSuccessStatusCode)
            {
                object contentValue = null;
                actionExecutedContext.Response.TryGetContentValue<object>(out contentValue);

                if (contentValue == null)
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Recurso no encontrado");
                }

                base.OnActionExecuted(actionExecutedContext);
            }
        }
    }
}