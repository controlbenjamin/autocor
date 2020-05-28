using Autocor.Catalogo.Web.Utils.Mensaje;
using Autocor.Catalogo.Web.Utils.Session;
using System.IO;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Extensions
{
    public static class ControllerExtension
    {
        public static void SetMensajeViewFromTemp(this Controller controller)
        {
            controller.ViewBag.Mensaje = controller.TempData[SessionConstants.Mensaje] as MensajeWeb;
        }

        public static void SetMensajeView(this Controller controller, MensajeWeb mensaje)
        {
            controller.ViewBag.Mensaje = mensaje;
        }

        public static void SetMensajeTemp(this Controller controller, MensajeWeb mensaje)
        {
            controller.TempData[SessionConstants.Mensaje] = mensaje;
        }

        public static string RenderRazorViewToString(this Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext,viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}