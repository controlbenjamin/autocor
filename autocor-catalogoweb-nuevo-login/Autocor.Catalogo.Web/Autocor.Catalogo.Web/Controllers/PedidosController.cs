using Autocor.Catalogo.Web.Models;
using Autocor.Catalogo.Web.Models.Filtros;
using Autocor.Catalogo.Web.Utils.Session;
using AutocorApi.Servicios.Core.Pedidos;
using AutocorApi.Servicios.Dto.Pedidos;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Autocor.Catalogo.Web.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        //  Servicios  //
        private IServicioConsultaPedidos _srvPedidos;

        //----------------------------//
        public PedidosController(IServicioConsultaPedidos srvPedidos)
        {
            this._srvPedidos = srvPedidos;
        }

        // GET: Pedidos
        public ActionResult Index(FiltroPedidosModel filtroPedidos)
        {
            var codigoCliente = SessionManager.Current.Usuario.Codigo;
            //Consultar las fechas si llevan o no filtro
            var desde = DateTime.Now.AddDays(-60);
           
            var pedidosResult = _srvPedidos.Buscar(codigoCliente, fechaDesde: desde, pagina: filtroPedidos.Pagina, tamanoPagina: 50);

            var pedidos = AutoMapper.Mapper.Map<IEnumerable<PedidoDto>>(pedidosResult.Items);

            var model = new PedidosViewModel
            {
                FiltroPedidos = filtroPedidos,
                Pedidos = pedidos
            };

            return View(model);
        }
    }
}