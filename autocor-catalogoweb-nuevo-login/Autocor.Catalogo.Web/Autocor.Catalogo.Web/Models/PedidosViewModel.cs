using Autocor.Catalogo.Web.Models.Filtros;
using AutocorApi.Servicios.Dto.Pedidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autocor.Catalogo.Web.Models
{
    public class PedidosViewModel
    {
        public FiltroPedidosModel FiltroPedidos { get; set; }
        public IEnumerable<PedidoDto> Pedidos { get; set; }
    }
}