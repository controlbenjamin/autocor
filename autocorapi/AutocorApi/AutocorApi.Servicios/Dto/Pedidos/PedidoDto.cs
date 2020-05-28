using AutocorApi.Servicios.Dto.Clientes;
using System;
using System.Collections.Generic;

namespace AutocorApi.Servicios.Dto.Pedidos
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public int CodigoCliente { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public int IdEstadoPedido { get; set; }
        public decimal ImporteTotal { get; set; }
        public string Observaciones { get; set; }
        public int NumeroPedidoSistema { get; set; }

        public virtual ClienteMinDto Cliente { get; set; }
        public virtual EstadoPedidoDto Estado { get; set; }

        public virtual IEnumerable<DetallePedidoDto> Detalles { get; set; }
    }
}