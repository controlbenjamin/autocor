using System.Web.Http;
using AutocorApi.Servicios.Core;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/stock")]
    public class StockController : ApiController
    {
        private IServicioProductos _srvProductos;

        public StockController(IServicioProductos srvProductos)
        {
            this._srvProductos = srvProductos;
        }

        [HttpGet, Route("{codigoPieza}")]
        public IHttpActionResult GetStock(string codigoPieza)
        {
            return Ok(_srvProductos.ObtenerStockActual(codigoPieza).StockReal);
        }
    }
}