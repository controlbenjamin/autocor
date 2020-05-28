using System;
using System.Collections.Generic;

namespace AutocorApi.Entidades
{
    public class Pedido
    {
        public int Id { get; set; }
        public int CodigoCliente { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public int IdEstadoPedido { get; set; }
        public decimal ImporteTotal { get; set; }
        public string Observaciones { get; set; }
        public int NumeroPedidoSistema { get; set; }

        public virtual ClienteBase Cliente { get; set; }
        public virtual EstadoPedido Estado { get; set; }

        public virtual IEnumerable<DetallePedido> Detalles { get; set; }
    }
}