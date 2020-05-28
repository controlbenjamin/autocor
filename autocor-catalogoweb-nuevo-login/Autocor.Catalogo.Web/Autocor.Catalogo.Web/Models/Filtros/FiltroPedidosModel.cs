using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autocor.Catalogo.Web.Models.Filtros
{
    public class FiltroPedidosModel
    {
        public int Pagina { get; set; }
        public int Estado { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }


        public FiltroPedidosModel()
        {
            this.Pagina = 1;
        }
    }
}

