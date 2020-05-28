using System;
using System.Collections.Generic;
using System.Linq;

namespace AutocorApi.Servicios.Dto.Pedidos
{
    public class NuevoPedidoDto
    {
        public NuevoPedidoDto()
        {
            Detalles = new List<NuevoDetallePedidoDto>();
        }

        public int CodigoCliente { get; set; }
        public string Observaciones { get; set; }
        // public decimal ImporteTotal => Detalles.Sum(d => d.SubTotal);

        public virtual IEnumerable<NuevoDetallePedidoDto> Detalles { get; set; }
    }
}