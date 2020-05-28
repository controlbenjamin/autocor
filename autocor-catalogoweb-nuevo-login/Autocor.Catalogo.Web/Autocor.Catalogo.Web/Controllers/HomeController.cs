using AutocorApi.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
      
        public ActionResult Index()
        {
            return View();

        }
    }
}