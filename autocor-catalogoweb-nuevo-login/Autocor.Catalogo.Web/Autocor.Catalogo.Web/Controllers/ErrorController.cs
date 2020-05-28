using Autocor.Catalogo.Web.Models;
using Autocor.Catalogo.Web.Models.Errores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Controllers
{
        public class ErrorController : Controller
        {
            public ActionResult RecursoNoEncontrado()
            {
                var model = new ErrorModelHttp(HttpStatusCode.NotFound);
            if (User.Identity.IsAuthenticated)
            {
                return View("Error", model);
            }
            return View("Error2", model);
              
            }

            public ActionResult PeticionInvalida()
            {
                var model = new ErrorModelHttp(HttpStatusCode.BadRequest);
            if (User.Identity.IsAuthenticated)
            {
                return View("Error", model);
            }
            return View("Error2", model);
        }

            public ActionResult ServerError()
            {
                var model = new ErrorModelHttp(HttpStatusCode.InternalServerError);
                if (User.Identity.IsAuthenticated)
                {
                    return View("Error", model);
                }
                return View("Error2", model);
            }

        }
}