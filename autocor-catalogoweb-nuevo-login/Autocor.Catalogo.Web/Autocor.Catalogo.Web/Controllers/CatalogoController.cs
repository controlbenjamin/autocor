using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Autocor.Catalogo.Web.Extensions;
using Autocor.Catalogo.Web.Models;
using Autocor.Catalogo.Web.Models.Filtros;
using Autocor.Catalogo.Web.Utils.Mensaje;
using Autocor.Catalogo.Web.Utils.Session;
using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto.Productos;
using AutocorApi.Servicios.Dto.Utils;
using AutoMapper;

namespace Autocor.Catalogo.Web.Controllers
{
    [Authorize]
    public class CatalogoController : Controller
    {
        private IServicioProductos _srvProductos;
        private IServicioMarcas _srvMarcas;
        private IServicioRubros _srvRubros;
        private IServicioTiposAuto _srvTipoAuto;
        private IServicioUsuariosWeb _srvUsuariosWeb;

        public CatalogoController(IServicioProductos srvProductos, IServicioMarcas srvMarcas, IServicioRubros srvRubros, IServicioTiposAuto srvTipoAuto, IServicioUsuariosWeb srvUsuariosWeb)
        {
            this._srvProductos = srvProductos;
            this._srvMarcas = srvMarcas;
            this._srvRubros = srvRubros;
            this._srvTipoAuto = srvTipoAuto;
            this._srvUsuariosWeb = srvUsuariosWeb;
        }

        public ActionResult Index(FiltroProductoModel filtroProducto)
        {

            return RedirectToAction("CerrarSesion", "Login");
            //var cliente = SessionManager.Current.Usuario.Codigo; // dragon

            //var estadoUsuario = _srvUsuariosWeb.CheckEstado(cliente.ToString());

            //if (!estadoUsuario)
            //{
            //    return RedirectToAction("CerrarSesion", "Login");
            //}

            PagedResultDto<ProductoDto> productoResult;
            bool ultimaPagina = true;

            if (filtroProducto != null && filtroProducto.BuscaProducto())
            {
                // buscar productos
                productoResult = _srvProductos.ObtenerProductos(filtroProducto.CriterioBusqueda, filtroProducto.CodigoRubro, filtroProducto.CodigoMarca, filtroProducto.CodigoTipoAuto, pagina: filtroProducto.Pagina, tamanoPagina: 50);
                ultimaPagina = productoResult.PageNumber == productoResult.PageCount;
            }
            else
            {
                productoResult = new PagedResultDto<ProductoDto>(Enumerable.Empty<ProductoDto>(), 1, 0, 0);
            }


            var productos = Mapper.Map<IEnumerable<ProductoBaseDto>>(productoResult.Items);

            if (Request.IsAjaxRequest())
            {
                //return PartialView("_FilaProducto", productos);
                if (Request.Browser.IsMobileDevice)
                {
                    //return PartialView("_CardsProductos", productos);

                    return Json(new MensajeDataSimple(new
                    {
                        ProductosHtmlMobile = this.RenderRazorViewToString("_CardsProductos", productos),
                        UltimaPagina = ultimaPagina
                    }), JsonRequestBehavior.AllowGet);
                }

                return Json(new MensajeDataSimple(new
                {
                    Productos = productos,
                    UltimaPagina = ultimaPagina
                }), JsonRequestBehavior.AllowGet);
            }

            // llenar los filtros

            //rubros
            filtroProducto.Rubros = _srvRubros.ObtenerRubros();

            // marcas
            if (filtroProducto.CodigoRubro.HasValue)
            {
                if (string.IsNullOrEmpty(filtroProducto.CodigoMarca))
                {
                    filtroProducto.Marcas = _srvMarcas.ObtenerMarcasPorRubro(filtroProducto.CodigoRubro.Value);
                }
                else
                {
                    filtroProducto.Marcas = _srvMarcas.ObtenerMarcas();
                }
            }
            else
            {
                filtroProducto.Marcas = _srvMarcas.ObtenerMarcas();
            }

            // tipos auto
            if (!string.IsNullOrEmpty(filtroProducto.CodigoMarca))
            {
                filtroProducto.Tipos = _srvTipoAuto.ObtenerTiposAutoPorMarca(filtroProducto.CodigoMarca);
            }
            else
            {
                filtroProducto.Tipos = _srvTipoAuto.ObtenerTiposAuto();
            }

            var model = new ProductosViewModel
            {
                FiltroProductos = filtroProducto,
                Productos = productos,
                UltimaPagina = ultimaPagina
            };

            //if(model.Productos.Count() == 0 )
            //{
            //    this.SetMensajeView(new MensajeWeb("El producto no existe en el catálogo.", "Aviso","Alerta"));
            //}

            string view = Request.Browser.IsMobileDevice ? "IndexMobile" : "Index";
            return View(view, model);
        }

        public ActionResult MarcasPorRubro(int codigoRubro)
        {
            var marcas = _srvMarcas.ObtenerMarcasPorRubro(codigoRubro);
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem { Text = "Todos", Value = "" });
            foreach (var item in marcas)
            {
                states.Add(new SelectListItem { Text = item.Descripcion, Value = item.Codigo });
            }

            return Json(new SelectList(states, "Value", "Text"));
        }

        public ActionResult TipoAutoPorMarca(string codigoMarca)
        {
            var tipo = _srvTipoAuto.ObtenerTiposAutoPorMarca(codigoMarca);
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem { Text = "Todos", Value = "" });
            foreach (var item in tipo)
            {
                states.Add(new SelectListItem { Text = item.Descripcion, Value = item.Codigo.ToString() });
            }
            return Json(new SelectList(states, "Value", "Text"));
        }

        [HttpGet]
        public ActionResult Equivalencias(string codigoPieza)
        {
            if (string.IsNullOrEmpty(codigoPieza))
                throw new HttpException((int)HttpStatusCode.BadRequest, "No se especificó código de pieza");

            var productosEquivalentes = _srvProductos.ObtenerProductosEquivalentes(codigoPieza);

            if (Request.IsAjaxRequest())
                return Json(productosEquivalentes, JsonRequestBehavior.AllowGet);

            if (!Request.Browser.IsMobileDevice)
                return RedirectToAction("Index", new { CriterioBusqueda = codigoPieza });

            var model = new ProductosViewModel()
            {
                Productos = productosEquivalentes
            };

            return View("EquivalenciasMobile", model);
        }

        [HttpGet]
        public ActionResult ObtenerStock(string codigoPieza)
        {
            if (string.IsNullOrEmpty(codigoPieza))
                throw new HttpException((int)HttpStatusCode.BadRequest, "No se especificó código de pieza");

            var stock = _srvProductos.ObtenerStockActual(codigoPieza);
            return Json(stock, JsonRequestBehavior.AllowGet);
        }
    }
}